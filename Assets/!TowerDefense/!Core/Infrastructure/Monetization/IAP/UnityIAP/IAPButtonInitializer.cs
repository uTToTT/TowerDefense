using UnityEngine;

namespace TToTT.Core.Purchasing
{
    public class IAPButtonInitializer : MonoBehaviour
    {
        public void Initialize(UnityIAP5Service iapService)
        {
            var buttons = FindObjectsByType<ProductPurchaseButtonHelper>(FindObjectsSortMode.None);

            foreach (var button in buttons)
                button.Initialize(iapService);

            Debug.Log($"[IAPButtonInitializer] Initialized {buttons.Length} IAP buttons");
        }
    }
}