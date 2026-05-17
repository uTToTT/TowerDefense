using System;

public interface IAdsService
{
    void LoadRewardedAd();
    void ShowRewardedAd(string placement, Action onRewarded);
}