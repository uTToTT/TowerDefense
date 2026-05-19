using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TToTT.Core.Purchasing
{
    [RequireComponent(typeof(Button))]
    public class ProductPurchaseButtonHelper : MonoBehaviour
    {
        private UnityIAP5Service _IAPService;

        public string productId;
        public bool consumePurchase = true;
        public TMP_Text titleText;
        public TMP_Text descriptionText;
        public TMP_Text priceText;

        private bool _initialized;

        public void Initialize(UnityIAP5Service iapService)
        {
            _IAPService = iapService;
            _initialized = true;

            if (gameObject.activeInHierarchy)
            {
                _IAPService.RegisterButton(this);
                UpdateText();
            }
        }

        void Start()
        {
            if (!_initialized)
                Debug.LogError($"[{nameof(ProductPurchaseButtonHelper)}] Not initialized on {gameObject.name}. Call Initialize() first.");

            ConfigureButton();
        }

        private void ConfigureButton()
        {
            var button = GetComponent<Button>();
            button?.onClick.AddListener(PurchaseProduct);

            if (string.IsNullOrEmpty(productId))
                Debug.LogError($"[{nameof(ProductPurchaseButtonHelper)}] productId is empty on {gameObject.name}");
        }

        void PurchaseProduct() => _IAPService?.InitiatePurchase(productId);

        void OnEnable()
        {
            if (!_initialized) return;
            _IAPService?.RegisterButton(this);
            UpdateText();
        }

        void OnDisable() => _IAPService?.UnregisterButton(this);

        internal void UpdateText()
        {
            var product = _IAPService?.FindProduct(productId);
            if (product == null) return;

            if (titleText != null)
                titleText.text = product.metadata.localizedTitle;
            if (descriptionText != null)
                descriptionText.text = product.metadata.localizedDescription;
            if (priceText != null)
                priceText.text = product.metadata.localizedPriceString;
        }
    }
}