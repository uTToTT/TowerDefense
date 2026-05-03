using TToTT.Core.DI;
using TToTT.Core.Installers;

namespace TToTT.TowerDefense.Installers
{
    public class EconomyInstaller : IInstaller
    {
        private readonly ShopConfig _shopConfig;
        private readonly ProductSlot[] _productSlots;
        private readonly ButtonWrapper _reroll;

        public EconomyInstaller(
            ShopConfig shopConfig,
            ProductSlot[] productSlots,
            ButtonWrapper reroll)
        {
            _shopConfig = shopConfig;
            _productSlots = productSlots;
            _reroll = reroll;
        }

        public void Install(DIContainer container)
        {
            container.BindInstance(_shopConfig);

            container.Bind<Wallet>(Lifetime.Singleton);
            container.Bind<EconomyController>(Lifetime.Singleton);
            container.Bind<ShopController>(Lifetime.Singleton);

            container.Resolve<ShopController>().Init(_productSlots, _reroll);
        }
    }
}