using System;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public class MapDataService : IDisposable
    {
        private readonly MapValidator _validator;

        private MapData _data;
        private CellData[,] _mapCell;

        public float CellSize => _data.cellSize;
        public int Width => _data.width;
        public int Height => _data.height;

        #region Init

        public MapDataService(MapValidator validator)
        {
            _validator = validator;
        }

        public void Dispose()
        {
            _data = null;
            _mapCell = null;
        }

        public void SetMapData(MapData data)
        {
            _data = data;
            _mapCell = new CellData[data.width, data.height];
        }

        #endregion

        public void SetCellType(int x, int y, CellType type)
        {
            var data = GetCellData(x, y);
            data.CellType = type;
        }

        public void SetCellBusyState(int x, int y, bool state)
        {
            var data = GetCellData(x, y);
            data.IsBusy = state;
        }

        public CellData GetCellData(int x, int y)
        {
            if (!_validator.IsInside(x, y))
                return null;

            if (_mapCell[x, y] == null) 
                _mapCell[x, y] = new CellData();

            return _mapCell[x, y];
        }

        public CellData GetCellData(Vector2Int v2Int) =>
            GetCellData(v2Int.x, v2Int.y);

        public void RegisterMapObject(Vector2Int pos, MapObject mapObject)
        {
            var occupiedPoss = MapUtils.GetOccupiedCells(pos, mapObject.Shape);

            foreach (var p in occupiedPoss)
            {
                var cell = GetCellData(p);
                if (cell != null && mapObject != null)
                {
                    cell.MapObject = mapObject;
                    cell.IsBusy = true;
                }
            }
        }

        public void UnregisterMapObject(MapObject mapObject)
        {
            var occupiedPoss = MapUtils.GetOccupiedCells(mapObject.MapPos, mapObject.Shape);

            foreach (var p in occupiedPoss)
            {
                var cell = GetCellData(p);
                if (cell != null)
                {
                    cell.MapObject = null;
                    cell.IsBusy = false;
                }
            }
        }

        public bool HasObject(CellData cell) => cell != null && cell.MapObject != null;
        public bool HasObject(Vector2Int pos) => HasObject(GetCellData(pos));

        public bool TryGetObject(Vector2Int mapPos, out MapObject mapObject) =>
            TryGetObject(GetCellData(mapPos), out mapObject);
        public bool TryGetObject(CellData cellData, out MapObject mapObject)
        {
            if (!HasObject(cellData))
            {
                mapObject = null;
                return false;
            }

            mapObject = cellData.MapObject;
            return true;
        }

        public Route GetRoute(RouteId id) => _data.GetRoute(id);
    }
}