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

        public GameInstaller(CellFactoryRegistry cellFactoryRegistry, Grid grid)
        {
            _cellFactoryRegistry = cellFactoryRegistry;
            _grid = grid;
        }

        public void Install(DIContainer container)
        {
            new MapInstaller(_cellFactoryRegistry, _grid).Install(container);

            container.Bind<PlayerInputController>(Lifetime.Singleton);
            container.Bind<TowerManager>(Lifetime.Singleton);
            container.Bind<EconomyController>(Lifetime.Singleton);
            container.Bind<EnemyManager>(Lifetime.Singleton);
            container.Bind<ProductShopController>(Lifetime.Singleton);
            container.Bind<ObjectSelector>(Lifetime.Singleton);
            container.Bind<ParticlesGenerator>(Lifetime.Singleton);
            container.Bind<CameraShaker>(Lifetime.Singleton);
            container.Bind<GameLoop>(Lifetime.Singleton);
        }
    }
}