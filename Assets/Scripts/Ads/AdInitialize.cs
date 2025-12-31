using UnityEngine;
using GoogleMobileAds.Api;

public class AdInitialize : MonoBehaviour
{
    private void Awake()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }
}
