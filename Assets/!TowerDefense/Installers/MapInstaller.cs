using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Map;
using UnityEngine;

namespace TToTT.TowerDefense.Installers
{
    public class MapInstaller : IInstaller
    {
        private const string MAP_CONTAINER_NAME = "MapContainer";

        private readonly MapContext _ctx;

        public MapInstaller(MapContext ctx)
        {
            _ctx = ctx;
        }

        public void Install(DIContainer container)
        {
            // Scene objects
            var go = new GameObject(MAP_CONTAINER_NAME);
            container.BindInstance(go.AddComponent<CellContainer>());
            container.BindInstance<CellFactoryRegistry>(_ctx.CellFactory);
            container.BindInstance<MapObjectFactoryRegistry>(_ctx.ObjectFactory);
            container.BindInstance<MapObjectPreviewFactoryRegistry>(_ctx.PreviewFactory);
            container.BindInstance<Grid>(_ctx.Grid);
            container.BindInstance<MapRegistry>(_ctx.Maps);
            container.BindInstance<CellSelectionFactory>(_ctx.SelectionFactory);

            // Core
            container.Bind<MapBounds>(Lifetime.Singleton);
            container.Bind<MapDataService>(Lifetime.Singleton);
            container.Bind<MapValidator>(Lifetime.Singleton);

            // Grid
            container.Bind<GridController>(Lifetime.Singleton);
            container.Bind<SellectionController>(Lifetime.Singleton);

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