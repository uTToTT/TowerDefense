using System;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public class MapDataService : IDisposable
    {
        private readonly MapBounds _bounds;
        private readonly MapValidator _validator;

        private MapData _data;
        private CellData[,] _mapCell;

        public float CellSize => _data.cellSize;
        public int Width => _data.width;
        public int Height => _data.height;

        #region Init

        public MapDataService(MapValidator validator, MapBounds bounds)
        {
            _validator = validator;
            _bounds = bounds;
        }

        public void Dispose()
        {
            _data = null;
            _mapCell = null;
        }

        public void SetMapData(MapData data)
        {
            _data = data;
            _bounds.SetSize(_data.width, _data.height);
            _mapCell = new CellData[data.width, data.height];
        }

        #endregion

        public void SetCellType(int x, int y, CellType type)
        {
            var data = TryGetCellData(x, y);
            data.CellType = type;
        }

        public void SetCellBusyState(int x, int y, bool state)
        {
            var data = TryGetCellData(x, y);
            data.IsBusy = state;
        }

        public bool TryGetCellData(Vector2Int v2Int, out CellData cell) =>
            TryGetCellData(v2Int.x, v2Int.y, out cell);

        public bool TryGetCellData(int x, int y, out CellData cell)
        {
            if (_bounds.IsInside(x, y))
            {
                cell = null;
                return false;
            }

            _mapCell[x, y] ??= new CellData();
            cell = _mapCell[x, y];
            return true;
        }

        public void RegisterMapObject(Vector2Int pos, MapObject mapObject)
        {
            var occupiedPoss = MapUtils.GetOccupiedCells(pos, mapObject.Shape);

            foreach (var p in occupiedPoss)
            {
                if (!TryGetCellData(p, out var cell)) continue;
                cell.MapObject = mapObject;
                cell.IsBusy = true;
            }
        }

        public void UnregisterMapObject(MapObject mapObject)
        {
            var occupiedPoss = MapUtils.GetOccupiedCells(mapObject.MapPos, mapObject.Shape);

            foreach (var p in occupiedPoss)
            {
                if (!TryGetCellData(p, out var cell)) continue;
                cell.MapObject = null;
                cell.IsBusy = false;
            }
        }

        public bool HasObject(CellData cell) => cell != null && cell.MapObject != null;
        public bool HasObject(Vector2Int pos) => HasObject(TryGetCellData(pos));

        public bool TryGetObject(Vector2Int mapPos, out MapObject mapObject) =>
            TryGetObject(TryGetCellData(mapPos), out mapObject);
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