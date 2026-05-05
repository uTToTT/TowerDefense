using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Map;
using UnityEngine;

namespace TToTT.TowerDefense.Installers
{
    // TODO: implement Grid as IGrid to decouple Unity and domain
    public class MapInstaller : IInstaller
    {
        private const string MAP_CONTAINER_NAME = "MapContainer";

        private readonly CellFactoryRegistry _cellFactoryRegistry;
        private readonly Grid _grid;
        private readonly MapObjectFactoryRegistry _objFactoryRegistry;
        private readonly MapObjectPreviewFactoryRegistry _objPreviewFactoryRegistry;

        public MapInstaller(
            CellFactoryRegistry cellFactoryRegistry,
            Grid grid,
            MapObjectFactoryRegistry objectFactoryRegistry,
            MapObjectPreviewFactoryRegistry objectPreviewFactoryRegistry)
        {
            _cellFactoryRegistry = cellFactoryRegistry;
            _grid = grid;
            _objFactoryRegistry = objectFactoryRegistry;
            _objPreviewFactoryRegistry = objectPreviewFactoryRegistry;
        }

        public void Install(DIContainer container)
        {
            // Scene objects
            var go = new GameObject(MAP_CONTAINER_NAME);
            container.BindInstance(go.AddComponent<CellContainer>());
            container.BindInstance(_cellFactoryRegistry);
            container.BindInstance(_objFactoryRegistry);
            container.BindInstance(_objPreviewFactoryRegistry);
            container.BindInstance(_grid);

            // Core
            container.Bind<MapBounds>(Lifetime.Singleton);
            container.Bind<MapDataService>(Lifetime.Singleton);
            container.Bind<MapValidator>(Lifetime.Singleton);

            // Grid
            container.Bind<GridController>(Lifetime.Singleton);
            container.Bind<ObjectSelector>(Lifetime.Singleton);

            // Routes  
            container.Bind<MapRoutes>(Lifetime.Singleton);
            container.Bind<RouteController>(Lifetime.Singleton);

            // Map pipeline
            container.Bind<MapLoader>(Lifetime.Singleton);
            container.Bind<MapComposer>(Lifetime.Singleton);
            container.Bind<MapController>(Lifetime.Singleton);
            container.Bind<MapManager>(Lifetime.Singleton);
            container.Bind<BuildController>(Lifetime.Singleton);
            container.Bind<PlacementController>(Lifetime.Singleton);
        }
    }
}