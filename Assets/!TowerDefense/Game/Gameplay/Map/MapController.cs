using System;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    /// <summary>
    /// 
    /// TODO:
    /// - Route Controller/Provider
    /// - MapValidator                  done
    /// - MapDataController             done
    /// - Dispose()                     done
    /// 
    /// </summary>

    public class MapController : IDisposable
    {
        private readonly PlacementController _placementController;
        private readonly GridController _gridController;
        private readonly MapValidator _validator;
        private readonly MapDataService _dataService;

        #region Init

        public MapController(
            PlacementController placementController,
            GridController gridController,
            MapValidator mapValidator,
            MapDataService dataService)
        {
            _placementController = placementController;
            _gridController = gridController;
            _validator = mapValidator;
            _dataService = dataService;
        }

        public void Dispose()
        {
            _placementController.Dispose();
            _gridController.Dispose();
            _validator.Dispose();
            _dataService.Dispose();
        }

        #endregion

        public void SetMap(MapData mapData)
        {
            _dataService.SetMapData(mapData);
            _gridController.CenterGrid(mapData.width, mapData.height);
        }

        public bool TryPlaceObject(Vector2Int pos, MapObject mapObject)
        {
            if (!_validator.IsCellAvailable(pos)) return false;

            _dataService.RegisterMapObject(pos, mapObject);

            return true;
        }

        public void RemoveMapObject(MapObject mapObject)
        {
            _dataService.UnregisterMapObject(mapObject);
        }
    }
}