using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public class BuildController
    {
        private readonly MapObjectFactoryRegistry _factory;
        private readonly MapController _mapController;
        private readonly GridController _gridController;

        public BuildController(
            MapObjectFactoryRegistry factoryRegistry,
            MapController mapController,
            GridController gridController)
        {
            _factory = factoryRegistry;
            _factory.Init();
            _mapController = mapController;
            _gridController = gridController;
        }

        public bool TryBuild(MapObjectType type, Vector2Int pos, out MapObject obj)
        {
            obj = _factory.Create(type);

            if (!_mapController.TryPlaceObject(pos, obj)) return false;

            obj.MapPos = pos;
            obj.transform.position = MapUtils.MapToWorld(pos, _gridController.Grid);

            return true;
        }

        public void TearDown(MapObject mapObject)
        {
            _factory.Return(mapObject);
            _mapController.RemoveMapObject(mapObject);
        }
    }
}