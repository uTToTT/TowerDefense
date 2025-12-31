using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System;

public class Aid : MonoBehaviour
{
    [SerializeField] private Button _buttonRewardAid;

    //private string _rewardedUnitId = "ca-app-pub-2838363966752041/8615691612";
    private string _rewardedUnitId = "ca-app-pub-3940256099942544/5224354917"; // тест
    private int _counterWatchAd = 0;
    private bool _hasInternet;
    private RewardedAd _rewardedAd;

    private void FixedUpdate()
    {
        if (_rewardedAd.CanShowAd() && _counterWatchAd != 1)
        {
            _buttonRewardAid.interactable = true;
        }
        else
        {
            _buttonRewardAid.interactable = false;
        }
    }

    private void OnEnable()
    {
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        var adRequest = new AdRequest();

        RewardedAd.Load(_rewardedUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
                RegisterEventHandlers(_rewardedAd);
            });
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            GetAid();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    private void GetAid()
    {
        EventBus.onRewardPause?.Invoke();
        EventBus.onPanelLoseDisable?.Invoke();
        EventBus.onAid?.Invoke();
    }

    public void ShowAd()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
        {
            _counterWatchAd++;
            GetAid();
            return;
        }

        const string rewardMsg =
       "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                _counterWatchAd++;
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }
}
