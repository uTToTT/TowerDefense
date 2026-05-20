using GoogleMobileAds.Api;
using System;

public class AdMobAdsService : IAdsService
{
    private RewardedAd _rewardedAd;
    private readonly string _adUnitId =
#if UNITY_EDITOR
        "ca-app-pub-3940256099942544/5224354917";
#else
        "ca-app-pub-3940256099942544/5224354917";
#endif

    private readonly IAnalyticsService _analytics;

    public AdMobAdsService(IAnalyticsService analytics)
    {
        _analytics = analytics;
        MobileAds.Initialize(_ => LoadRewardedAd());
    }

    public void LoadRewardedAd()
    {
        RewardedAd.Load(_adUnitId, new AdRequest(), (ad, error) =>
        {
            if (error != null) return;
            _rewardedAd = ad;
        });
    }

    public void ShowRewardedAd(string placement, Action onRewarded)
    {
        if (_rewardedAd == null) return;

        //_rewardedAd.OnAdFullScreenContentClosed += ()=>
        //{
        //    OnAdDismissedFullScreenContent = _ => LoadRewardedAd();
        //};

        _rewardedAd.Show(_ =>
        {
            onRewarded?.Invoke();
            _analytics.TrackAdWatched(placement);
        });
    }
}
