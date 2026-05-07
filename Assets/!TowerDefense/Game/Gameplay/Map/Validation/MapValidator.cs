using System;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public class MapValidator : IDisposable
    {
        private readonly MapBounds _bounds;
        private readonly MapDataService _dataService;

        public MapValidator(MapDataService dataService, MapBounds bounds)
        {
            _dataService = dataService;
            _bounds = bounds;
        }

        public bool IsCellBusy(Vector2Int pos)
        {
            if (!_bounds.IsInside(pos)) return false;
            if (!_dataService.TryGetCellData(pos, out var cell)) return false;
            return cell != null && cell.IsBusy;
        }

        public bool IsCellAvailable(Vector2Int pos) =>
            _bounds.IsInside(pos) && !IsCellBusy(pos);



        public void Dispose() { }
    }
}