using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TToTT.Core.Purchasing
{
    [RequireComponent(typeof(Button))]
    public class ProductPurchaseButtonHelper : MonoBehaviour
    {
        private UnityIAP5Service _iapService;
        private IAPLogger _logger;

        public string productId;
        public bool consumePurchase = true;
        public TMP_Text titleText;
        public TMP_Text descriptionText;
        public TMP_Text priceText;

        private bool _initialized;

        #region Init

        public void Initialize(UnityIAP5Service iapService, IAPLogger logger)
        {
            _iapService = iapService;
            _logger = logger;
            _initialized = true;

            if (gameObject.activeInHierarchy)
            {
                _iapService.RegisterButton(this);
                UpdateText();
            }
        }

        #endregion

        #region Unity API

        void Start()
        {
            if (!_initialized)
                _logger.Log($"Not initialized on {gameObject.name}. Call Initialize() first.");

            ConfigureButton();
        }

        void OnEnable()
        {
            if (!_initialized) return;
            _iapService?.RegisterButton(this);
            UpdateText();
        }

        void OnDisable() => _iapService?.UnregisterButton(this);

        #endregion

        public void UpdateText()
        {
            var product = _iapService?.FindProduct(productId);
            if (product == null) return;

            if (titleText != null)
                titleText.text = product.metadata.localizedTitle;
            if (descriptionText != null)
                descriptionText.text = product.metadata.localizedDescription;
            if (priceText != null)
                priceText.text = product.metadata.localizedPriceString;
        }

        private void ConfigureButton()
        {
            var button = GetComponent<Button>();
            button?.onClick.AddListener(PurchaseProduct);

            if (string.IsNullOrEmpty(productId))
                _logger.Log($"ProductId is empty on {gameObject.name}");
        }

        private void PurchaseProduct() => _iapService?.InitiatePurchase(productId);
    }
}