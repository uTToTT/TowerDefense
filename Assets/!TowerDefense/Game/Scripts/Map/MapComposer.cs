using System;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public class MapComposer : IDisposable
    {
        private readonly CellFactoryRegistry _factories;
        private readonly CellContainer _cellContainer;
        private readonly GridController _gridController;

        public MapComposer(
            CellFactoryRegistry factories,
            CellContainer cellContainer,
            GridController gridController)
        {
            _factories = factories;
            _factories.Init();
            _cellContainer = cellContainer;
            _gridController = gridController;
        }

        public void Build(MapData map)
        {
            for (int y = 0; y < map.height; y++)
            {
                for (int x = 0; x < map.width; x++)
                {
                    var type = map.Get(x, y);
                    if (type == CellType.Empty) continue;

                    var cell = _factories.Create(type);
                    _cellContainer.SetChild(cell.transform);
                    cell.transform.position = _gridController.Grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
                }
            }
        }

        public void Dispose()
        {

        }
    }
}