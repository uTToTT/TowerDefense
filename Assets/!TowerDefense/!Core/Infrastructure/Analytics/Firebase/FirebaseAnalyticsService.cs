// Services/Analytics/FirebaseAnalyticsService.cs
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System;
using System.Data.Common;
using UnityEngine;

public class FirebaseAnalyticsService : IAnalyticsService
{
    private bool _isInitialized;

    public FirebaseAnalyticsService()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                _isInitialized = true;
            }
            else
            {
                Debug.LogError($"[FirebaseAnalytics] Failed to initialize: {task.Result}");
            }
        });
    }

    public void TrackWaveCompleted(int wave, int enemiesKilled)
    {
        if (!_isInitialized) return;

        FirebaseAnalytics.LogEvent("wave_completed",
            new Parameter("wave", wave),
            new Parameter("enemies_killed", enemiesKilled));
    }

    public void TrackAdWatched(string placement)
    {
        if (!_isInitialized) return;

        FirebaseAnalytics.LogEvent("ad_watched",
            new Parameter("placement", placement));
    }

    public void TrackPurchase(string productId)
    {
        if (!_isInitialized) return;

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPurchase,
            new Parameter(FirebaseAnalytics.ParameterItemID, productId),
            new Parameter(FirebaseAnalytics.ParameterSuccess, 1));
    }
}