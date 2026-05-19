using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TToTT.TowerDefense.Economy;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

namespace TToTT.Core.Purchasing
{
    // implement Analytics
    public class UnityIAP5Service : IIAPService
    {
        private IAPLogger _IAPLogger;

        IStoreService _storeService;
        IProductService _productService;
        IPurchaseService _purchaseService;
        IAnalyticsService _analytics;
        ICatalogProvider _catalog = new CatalogProvider();
        CrossPlatformValidator _crossPlatformValidator;

        private readonly List<ProductPurchaseButtonHelper> _activePurchaseButtons = new List<ProductPurchaseButtonHelper>();
        private readonly IAPPaywallCallbacks _callbacks;
        private readonly EconomyController _economy;

        public bool IsNoAdsPurchased { get; set; }

        public UnityIAP5Service(
            IAnalyticsService analytics,
            EconomyController economy,
            IAPButtonInitializer buttonInitializer,
            IAPLogger logger)
        {
            buttonInitializer.Initialize(this);
            _IAPLogger= logger;
            _callbacks = new IAPPaywallCallbacks(this, _IAPLogger);
            _analytics = analytics;
            _economy = economy;

            CreateServices();

            InitCatalog();
            InitializeIapService();
            CreateCrossPlatformValidator();

            ConnectToStore();
        }

        private void InitCatalog()
        {
            var initialProductsToFetch = new List<ProductDefinition>
            {
                new ProductDefinition(ProductIds.GoldPack, ProductType.Consumable),
                new ProductDefinition(ProductIds.NoAds, ProductType.NonConsumable)
            };

            _catalog.AddProducts(initialProductsToFetch);
        }

        private void CreateServices()
        {
            _storeService = UnityIAPServices.DefaultStore();
            _productService = UnityIAPServices.DefaultProduct();
            _purchaseService = UnityIAPServices.DefaultPurchase();

            ConfigureServiceCallbacks();
        }

        private void ConfigureServiceCallbacks()
        {
            ConfigureProductServiceCallbacks();
            ConfigurePurchasingServiceCallbacks();
        }

        private void ConfigureProductServiceCallbacks()
        {
            _productService.OnProductsFetched += _callbacks.OnInitialProductsFetched;
            _productService.OnProductsFetchFailed += _callbacks.OnInitialProductsFetchFailed;
        }

        private void ConfigurePurchasingServiceCallbacks()
        {
            _purchaseService.OnPurchasesFetched += _callbacks.OnExistingPurchasesFetched;
            _purchaseService.OnPurchasesFetchFailed += _callbacks.OnExistingPurchasesFetchFailed;
            _purchaseService.OnPurchasePending += _callbacks.OnPurchasePending;
            _purchaseService.OnPurchaseConfirmed += _callbacks.OnPurchaseConfirmed;
            _purchaseService.OnPurchaseFailed += _callbacks.OnPurchaseFailed;
            _purchaseService.OnPurchaseDeferred += _callbacks.OnOrderDeferred;
        }

        public void UpdateActivePurchaseButtons()
        {
            foreach (var button in _activePurchaseButtons)
            {
                button.UpdateText();
            }
        }

        public void FetchExistingPurchases()
        {
            _purchaseService.FetchPurchases();
        }

        public void RestorePurchases()
        {
            _purchaseService.RestoreTransactions(OnTransactionsRestored);
        }

        private void OnTransactionsRestored(bool success, string error)
        {
            _IAPLogger.LogConsole("Transactions restored: " + success);
        }

        public static bool IsReceiptAvailable(Orders existingOrders)
        {
            return existingOrders != null &&
                   (existingOrders.ConfirmedOrders.Any(order => !string.IsNullOrEmpty(order.Info.Receipt)) ||
                    existingOrders.PendingOrders.Any(order => !string.IsNullOrEmpty(order.Info.Receipt)));
        }

        private void InitializeIapService()
        {
            IAPService.Initialize(OnServiceInitialized, (message) =>
            {
                _IAPLogger.LogConsole($"Initialization failed, IAP service dependency error: {message}");
            });
        }

        private void CreateCrossPlatformValidator()
        {
#if !UNITY_EDITOR
            try
            {
                if (CanCrossPlatformValidate())
                {
#if !DEBUG_STOREKIT_TEST
                    _crossPlatformValidator = new CrossPlatformValidator(GooglePlayTangle.Data(), Application.identifier);
#else
                    _crossPlatformValidator = new CrossPlatformValidator(GooglePlayTangle.Data(), Application.identifier);
#endif
                }   
            }   
            catch (NotImplementedException exception)
            {
                _IAPLogger.LogConsole("===========");
                _IAPLogger.LogConsole($"Cross Platform Validator Not Implemented: {exception}");
            }
#endif
        }

        private void OnServiceInitialized()
        {
            _IAPLogger.LogConsole("Services Initialized.");
        }

        private async void ConnectToStore()
        {
            await _storeService.Connect();
            _IAPLogger.LogConsole("===========");
            _IAPLogger.LogConsole("Store Connected.");
            FetchInitialProducts();
        }

        private void FetchInitialProducts()
        {
            _catalog.FetchProducts(_productService.FetchProductsWithNoRetries, DefaultStoreHelper.GetDefaultStoreName());
        }

        public void InitiatePurchase(string productId)
        {
            var product = FindProduct(productId);

            if (product != null)
            {
                _purchaseService?.PurchaseProduct(product);
            }
            else
            {
                _IAPLogger.LogConsole($"The product service has no product with the ID {productId}");
            }
        }

        public Product FindProduct(string productId)
        {
            return GetFetchedProducts()?.FirstOrDefault(product => product.definition.id == productId);
        }

        public ReadOnlyObservableCollection<Product> GetFetchedProducts()
        {
            return _productService?.GetProducts();
        }

        public void ConfirmOrderIfAutomatic(PendingOrder order)
        {
            if (ShouldConfirmOrderAutomatically(order))
            {
                ConfirmOrder(order);
            }
        }

        private bool ShouldConfirmOrderAutomatically(PendingOrder order)
        {
            var containsItemToNotAutoConfirm = false;
            var containsItemToAutoConfirm = false;

            foreach (var cartItem in order.CartOrdered.Items())
            {
                var matchingButton = FindMatchingButtonByProduct(cartItem.Product.definition.id);

                if (matchingButton)
                {
                    if (matchingButton.consumePurchase)
                    {
                        containsItemToAutoConfirm = true;
                    }
                    else
                    {
                        containsItemToNotAutoConfirm = true;
                    }
                }
            }

            if (containsItemToNotAutoConfirm && containsItemToAutoConfirm)
            {
                _IAPLogger.LogConsole("===========");
                _IAPLogger.LogConsole("Pending Order contains some products to not confirm. Confirming by default!");
            }

            return containsItemToAutoConfirm;
        }

        private ProductPurchaseButtonHelper FindMatchingButtonByProduct(string productId)
        {
            foreach (var button in _activePurchaseButtons)
            {
                if (button.productId == productId)
                {
                    return button;
                }
            }

            return null;
        }

        private void ConfirmOrder(PendingOrder pendingOrder)
        {
            _purchaseService.ConfirmPurchase(pendingOrder);
        }

        public void RegisterButton(ProductPurchaseButtonHelper button)
        {
            _activePurchaseButtons.Add(button);
        }

        public void UnregisterButton(ProductPurchaseButtonHelper button)
        {
            _activePurchaseButtons.Remove(button);
        }

        public void ConfirmPendingPurchaseForId(string id)
        {
            var product = FindProduct(id);
            var order = product != null ? GetPendingOrder(product) : null;

            if (order != null)
            {
                ConfirmOrder(order);
            }
        }

        private PendingOrder GetPendingOrder(Product product)
        {
            var orders = _purchaseService.GetPurchases();

            foreach (var order in orders)
            {
                if (order is PendingOrder pendingOrder &&
                    pendingOrder.CartOrdered.Items().First()?.Product.definition.storeSpecificId == product.definition.storeSpecificId)
                {
                    return pendingOrder;
                }
            }

            return null;
        }

        public void ValidatePurchaseIfPossible(IOrderInfo orderInfo)
        {
            if (CanCrossPlatformValidate())
            {
                ValidatePurchase(orderInfo);
            }
        }

        private bool CanCrossPlatformValidate()
        {
            return IsGooglePlay() ||
                   Application.platform == RuntimePlatform.IPhonePlayer ||
                   Application.platform == RuntimePlatform.OSXPlayer ||
                   Application.platform == RuntimePlatform.tvOS;
        }

        private void ValidatePurchase(IOrderInfo orderInfo)
        {
            try
            {
                var result = _crossPlatformValidator.Validate(orderInfo.Receipt);

                if (IsGooglePlay())
                {
                    _IAPLogger.LogConsole("Validated Receipt. Contents:");
                    foreach (IPurchaseReceipt productReceipt in result)
                    {
                        _IAPLogger.LogReceiptValidation(productReceipt);
                    }
                }
                else
                {
                    _IAPLogger.LogConsole("Validated Receipt.");
                }
            }
            catch (IAPSecurityException ex)
            {
                _IAPLogger.LogConsole("Invalid receipt, not unlocking content. " + ex);
            }
        }

        private bool IsGooglePlay()
        {
            return Application.platform == RuntimePlatform.Android && DefaultStoreHelper.GetDefaultStoreName() == UnityEngine.Purchasing.GooglePlay.Name;
        }

        public void GetNoAds()
        {
            IsNoAdsPurchased = true;
        }

        public void GetGoldPack()
        {
            _economy.AddMoney(100);
        }
    }
}
