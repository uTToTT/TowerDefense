using System;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    /// <summary>
    /// 
    /// TODO:
    /// - Route Controller/Provider     DONE
    /// - MapValidator                  DONE
    /// - MapDataController             DONE
    /// - Dispose()                     DONE
    /// 
    /// </summary>

    public class MapController : IDisposable
    {
        private readonly GridController _gridController;
        private readonly MapValidator _validator;
        private readonly MapDataService _dataService;
        private readonly RouteController _routeController;

        #region Init

        public MapController(
            GridController gridController,
            MapValidator mapValidator,
            MapDataService dataService,
            RouteController routeController)
        {
            _gridController = gridController;
            _validator = mapValidator;
            _dataService = dataService;
            _routeController = routeController;
        }

        public void Dispose()
        {
            _gridController.Dispose();
            _validator.Dispose();
            _dataService.Dispose();
        }

        #endregion

        public void SetMap(MapData mapData)
        {
            _dataService.SetMapData(mapData);
            _routeController.SetRoutes(mapData);
            _gridController.CenterGrid();
        }

        public bool TryPlaceObject(Vector2Int pos, MapObject mapObject)
        {
            if (!_validator.IsCellAvailable(pos)) return false;

            _dataService.RegisterMapObject(pos, mapObject);

            return true;
        }

        public bool IsAreaAvailable(Vector2Int pos, MapObjectShape shape)
        {
            var occupiedCells = MapUtils.GetOccupiedCells(pos, shape);

            foreach (var cell in occupiedCells)
            {
                if (!IsCellAvailable(cell)) return false;
            }

            return true;
        }

        public bool IsCellAvailable(Vector2Int pos) => _validator.IsCellAvailable(pos);

        public void RemoveMapObject(MapObject mapObject)
        {
            _dataService.UnregisterMapObject(mapObject);
        }

        public void InitCellStates(MapData map)
        {
            for (int y = 0; y < map.height; y++)
            {
                for (int x = 0; x < map.width; x++)
                {
                    var type = map.Get(x, y);
                    if (type == CellType.Empty) continue;

                    bool isBlocked = type.IsBlocked();
                    _dataService.SetCellBusyState(x, y, isBlocked);
                }
            }
        }

    }
}