using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.Core.Purchasing;

public class MonetizationInstaller : IInstaller
{
    private readonly MonetizationContext _ctx;

    public MonetizationInstaller(MonetizationContext ctx)
    {
        _ctx = ctx;
    }

    public void Install(DIContainer container)
    {
        container.Bind<IAdsService, AdMobAdsService>(Lifetime.Singleton);
        container.Bind<IIAPService, UnityIAP5Service>(Lifetime.Singleton);
        container.Bind<IAPLogger>(Lifetime.Singleton);
        container.BindInstance<IAPButtonInitializer>(_ctx.IAPButtonInitializer);

#if UNITY_EDITOR
        container.Bind<IAnalyticsService, DebugAnalyticsService>(Lifetime.Singleton);
#else
        container.Bind<IAnalyticsService, FirebaseAnalyticsService>(Lifetime.Singleton);
#endif
    }
}