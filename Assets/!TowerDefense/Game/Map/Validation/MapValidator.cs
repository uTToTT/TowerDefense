using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace TToTT.TowerDefense.Map
{
    public class MapValidator
    {
        private readonly MapDataService _dataService;

        public MapValidator(MapDataService dataService)
        {
            _dataService = dataService;
        }

        public bool IsInside(Vector2Int pos) => IsInside(pos.x, pos.y);
        public bool IsInside(int x, int y) =>
            x >= 0 && y >= 0 &&
            x < _dataService.Width && y < _dataService.Height;

        public bool IsCellBusy(Vector2Int pos)
        {
            var cell = _dataService.GetCellData(pos);
            return cell != null && cell.IsBusy;
        }
    }
}