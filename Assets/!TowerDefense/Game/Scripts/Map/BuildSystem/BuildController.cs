using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public class BuildController
    {
        private readonly MapObjectFactoryRegistry _factory;
        private readonly MapController _mapController;

        public BuildController(
            MapObjectFactoryRegistry factoryRegistry,
            MapController mapController)
        {
            _factory = factoryRegistry;
            _factory.Init();
            _mapController = mapController;
        }

        public bool TryBuild(MapObjectType type, Vector2Int pos, out MapObject obj)
        {
            obj = _factory.Create(type);

            if (!_mapController.TryPlaceObject(pos, obj)) return false;

            obj.SetPosition(pos);

            return true;
        }

        public void TearDown(MapObject mapObject)
        {
            _factory.Return(mapObject);
            _mapController.RemoveMapObject(mapObject);
        }
    }
}