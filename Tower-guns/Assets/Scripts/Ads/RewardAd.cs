using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;

public class RewardAd : MonoBehaviour
{
    [SerializeField] private int _moneyAdd = 100;
    [SerializeField] private GameObject _buttonRewardMoney;
    [SerializeField] private bool _adDisable;

    //private string _rewardedUnitId = "ca-app-pub-2838363966752041/6181099965";
    private string _rewardedUnitId = "ca-app-pub-3940256099942544/5224354917"; // тест
    private int _counterWatchAd = 0;
    private bool _hasInternet;

    private RewardedAd _rewardedAd;

    private void FixedUpdate()
    {
        if (!_adDisable)
        {
            if (_rewardedAd.CanShowAd() && _counterWatchAd != 3)
            {
                _buttonRewardMoney.SetActive(true);
            }
            else
            {
                _buttonRewardMoney.SetActive(false);
            }
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
            AddMoneyAds();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    private void AddMoneyAds()
    {
        EventBus.AddMoney?.Invoke(_moneyAdd);
        EventBus.onRewardPause?.Invoke();
    }

    public void ShowAd()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
        {
            _counterWatchAd++;
            AddMoneyAds();
            Debug.Log("Remove ads");
            return;
        }

        const string rewardMsg =
       "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";



        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                _counterWatchAd++;
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
            });

            StartCoroutine(TimerToLoadAd());
        }
    }

    IEnumerator TimerToLoadAd()
    {
        yield return new WaitForSeconds(20);
        LoadRewardedAd();
    }
}
