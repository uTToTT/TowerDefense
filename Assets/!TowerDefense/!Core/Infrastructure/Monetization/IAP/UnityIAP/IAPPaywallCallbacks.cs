using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace TToTT.Core.Purchasing
{
    public class IAPPaywallCallbacks
    {
        private readonly UnityIAP5Service _IAPService;
        private readonly IAPLogger _logger;

        public IAPPaywallCallbacks(
            UnityIAP5Service IAPservice,
            IAPLogger logger) 
        {
            _IAPService = IAPservice;
            _logger = logger;
        }

        public void OnInitialProductsFetched(List<Product> products)
        {
            _logger.LogFetchedProducts(products);
            _IAPService.UpdateActivePurchaseButtons();
            _IAPService.FetchExistingPurchases();
        }

        public void OnInitialProductsFetchFailed(ProductFetchFailed failure)
        {
            _logger.Log($"OnInitialProductsFetchFailed | {failure.FailureReason}");
        }

        public void OnExistingPurchasesFetched(Orders existingOrders)
        {
            var result = UnityIAP5Service.IsReceiptAvailable(existingOrders) ?
                "Success - Found Existing Orders with receipts" :
                "Notice: - No Existing Orders with receipts";

            _logger.Log($"OnExistingPurchasesFetched | result={result}");
        }

        public void OnExistingPurchasesFetchFailed(PurchasesFetchFailureDescription failure)
        {
            _logger.Log($"OnExistingPurchasesFetchFailed | msg{failure.Message}");
        }

        public void OnPurchasePending(PendingOrder order)
        {
            foreach (var cartItem in order.CartOrdered.Items())
            {
                var product = cartItem.Product;

                _logger.LogCompletedPurchase(product, order.Info);
            }

            _IAPService.ConfirmOrderIfAutomatic(order);
        }

        public void OnPurchaseConfirmed(Order order)
        {
            switch (order)
            {
                case FailedOrder failedOrder:
                    OnConfirmationFailed(failedOrder);
                    break;
                case ConfirmedOrder confirmedOrder:
                    OnPurchaseConfirmed(confirmedOrder);
                    break;
            }
        }

        void OnConfirmationFailed(FailedOrder failedOrder)
        {
            var reason = failedOrder.FailureReason;

            foreach (var cartItem in failedOrder.CartOrdered.Items())
            {
                _logger.LogFailedConfirmation(cartItem.Product, reason);
            }
        }

        public void OnPurchaseConfirmed(ConfirmedOrder order)
        {
            foreach (var cartItem in order.CartOrdered.Items())
            {
                var product = cartItem.Product;
                HandleConfirmedProduct(product);
                _logger.LogConfirmedOrder(product, order.Info);
            }
        }

        public void OnPurchaseFailed(FailedOrder failedOrder)
        {
            var reason = failedOrder.FailureReason;

            foreach (var cartItem in failedOrder.CartOrdered.Items())
            {
                _logger.LogFailedPurchase(cartItem.Product, reason);
            }
        }

        public void OnOrderDeferred(DeferredOrder deferredOrder)
        {
            foreach (var cartItem in deferredOrder.CartOrdered.Items())
            {
                _logger.LogDeferredPurchase(cartItem.Product);
            }
        }

        private void HandleConfirmedProduct(Product product)
        {
            switch (product.definition.id)
            {
                case ProductIds.NoAds:
                    _IAPService.GetNoAds();
                    break;

                case ProductIds.GoldPack:
                    _IAPService.GetGoldPack();
                    break;

                default:
                    _logger.Log($"Product is not defined | product_id={product.definition.id}");
                    break;
            }
        }
    }
}
