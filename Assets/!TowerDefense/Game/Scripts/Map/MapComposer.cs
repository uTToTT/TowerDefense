using System;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public class MapComposer : IDisposable
    {
        private readonly CellFactoryRegistry _factories;
        private readonly Transform _cellContainer;
        private readonly GridController _gridController;
        private readonly MapDataService _dataService;

        public MapComposer(
            CellFactoryRegistry factories,
            Transform cellContainer,
            MapDataService dataService,
            GridController gridController)
        {
            _factories = factories;
            _cellContainer = cellContainer;
            _dataService = dataService;
            _gridController = gridController;
        }

        public void Build(MapData map)
        {
            for (int y = 0; y < map.height; y++)
            {
                for (int x = 0; x < map.width; x++)
                {
                    var type = map.Get(x, y);
                    if (type == CellType.Empty)
                        continue;

                    var cell = _factories.Create(type);
                    cell.transform.SetParent(_cellContainer);

                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    cell.transform.position = _gridController.Grid.GetCellCenterWorld(cellPos);

                    _dataService.SetCellType(x, y, cell.CellType);

                    bool isBlocked =
                        cell.CellType == CellType.Path ||
                        cell.CellType == CellType.Entrance ||
                        cell.CellType == CellType.Exit ||
                        cell.CellType == CellType.Blocked;

                    _dataService.SetCellBusyState(x, y, isBlocked);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}