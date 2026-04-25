using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Map;
using UnityEngine;

namespace TToTT.TowerDefense.Installers
{
    // TODO: implement Grid as IGrid to decouple Unity and domain
    public class MapInstaller : IInstaller
    {
        private readonly CellFactoryRegistry _cellFactoryRegistry;
        private readonly Grid _grid;

        public MapInstaller(CellFactoryRegistry cellFactoryRegistry, Grid grid)
        {
            _cellFactoryRegistry = cellFactoryRegistry;
            _grid = grid;
        }

        public void Install(DIContainer container)
        {
            container.BindInstance(_cellFactoryRegistry);
            container.BindInstance(_grid);

            container.Bind<GridController>(Lifetime.Singleton);
            container.Bind<PlacementController>(Lifetime.Singleton);
            container.Bind<MapValidator>(Lifetime.Singleton);
            container.Bind<MapDataService>(Lifetime.Singleton);
            container.Bind<MapComposer>(Lifetime.Singleton);
            container.Bind<MapLoader>(Lifetime.Singleton);
            container.Bind<MapController>(Lifetime.Singleton);
            container.Bind<MapManager>(Lifetime.Singleton);
        }
    }
}