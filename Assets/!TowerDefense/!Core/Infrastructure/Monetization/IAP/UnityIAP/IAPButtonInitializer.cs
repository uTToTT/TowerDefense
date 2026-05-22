using UnityEngine;

namespace TToTT.Core.Purchasing
{
    public class IAPButtonInitializer : MonoBehaviour
    {
        public void Initialize(UnityIAP5Service iapService, IAPLogger logger)
        {
            var buttons = FindObjectsByType<ProductPurchaseButtonHelper>(FindObjectsSortMode.None);

            foreach (var button in buttons)
                button.Initialize(iapService, logger);

            logger.Log($"Initialized [{buttons.Length}] IAP buttons");
        }
    }
}