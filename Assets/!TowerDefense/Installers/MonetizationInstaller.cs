using TToTT.Core.DI;
using TToTT.Core.Installers;

public class MonetizationInstaller : IInstaller
{
    public void Install(DIContainer container)
    {
        container.Bind<IAdsService, AdMobAdsService>(Lifetime.Singleton);
        container.Bind<IIAPService, UnityIAPService>(Lifetime.Singleton);

#if UNITY_EDITOR
        container.Bind<IAnalyticsService, DebugAnalyticsService>(Lifetime.Singleton);
#else
        container.Bind<IAnalyticsService, FirebaseAnalyticsService>(Lifetime.Singleton);
#endif
    }
}