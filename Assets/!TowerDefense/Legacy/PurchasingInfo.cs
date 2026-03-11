using UnityEngine;
using UnityEngine.UI;

public class PurchasingInfo : MonoBehaviour
{
    [SerializeField] private GameObject _buttonRemoveAds;

    private void Start()
    {
        UpdateRemoveAdsButton();
    }

    public void UpdateRemoveAdsButton()
    {
        bool removeAds = PlayerPrefs.GetInt("RemoveAds") == 1;
        _buttonRemoveAds.SetActive(!removeAds);
    }

    private void OnEnable()
    {
        EventBus.onRemoveAds += UpdateRemoveAdsButton;
    }

    private void OnDisable()
    {
        EventBus.onRemoveAds -= UpdateRemoveAdsButton;
    }
}
