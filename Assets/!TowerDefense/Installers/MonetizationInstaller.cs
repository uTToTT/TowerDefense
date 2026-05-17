using TToTT.Core.DI;
using TToTT.Core.Installers;

public class MonetizationInstaller : IInstaller
{
    public void Install(DIContainer container)
    {
        container.Bind<IAdsService, AdMobAdsService>(Lifetime.Singleton);
        container.Bind<IIAPService, UnityIAPService>(Lifetime.Singleton);
    }
}