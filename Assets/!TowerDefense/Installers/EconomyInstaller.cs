using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Shop;

namespace TToTT.TowerDefense.Installers
{
    public class EconomyInstaller : IInstaller
    {
        private readonly ShopContext _ctx;

        public EconomyInstaller(ShopContext ctx)
        {
            _ctx = ctx;
        }

        public void Install(DIContainer container)
        {
            container.BindInstance<ShopConfig>(_ctx.Config);
            container.BindInstance<ButtonWrapper>(_ctx.RerollButton);
            container.BindInstance(new ShopSlots(_ctx.Slots));

            container.Bind<Wallet>(Lifetime.Singleton);
            container.Bind<EconomyController>(Lifetime.Singleton);
            container.Bind<ShopController>(Lifetime.Singleton);
        }
    }
}