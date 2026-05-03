using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Enemies;
using TToTT.TowerDefense.Towers;
using UnityEngine;

namespace TToTT.TowerDefense.Installers
{
    public class GameInstaller : IInstaller
    {
        private readonly CellFactoryRegistry _cellFactoryRegistry;
        private readonly Grid _grid;
        private readonly ShopConfig _shopConfig;
        private readonly ProductSlot[] _productSlots;
        private readonly ButtonWrapper _reroll;
        private readonly MapObjectFactoryRegistry _objectFactoryRegistry;

        public GameInstaller(
            CellFactoryRegistry cellFactoryRegistry,
            Grid grid,
            ShopConfig shopConfig,
            ProductSlot[] productSlots,
            ButtonWrapper reroll,
            MapObjectFactoryRegistry objectFactoryRegistry)
        {
            _cellFactoryRegistry = cellFactoryRegistry;
            _grid = grid;
            _shopConfig = shopConfig;
            _productSlots = productSlots;
            _reroll = reroll;
            _objectFactoryRegistry = objectFactoryRegistry;
        }

        public void Install(DIContainer container)
        {
            new InputInstaller().Install(container);
            new VFXInstaller().Install(container);
            new MapInstaller(_cellFactoryRegistry, _grid, _objectFactoryRegistry).Install(container);
            new EconomyInstaller(_shopConfig, _productSlots, _reroll).Install(container);

            container.Bind<TowerManager>(Lifetime.Singleton);
            container.Bind<EnemyManager>(Lifetime.Singleton);
            container.Bind<GameLoop>(Lifetime.Singleton);
        }
    }
}