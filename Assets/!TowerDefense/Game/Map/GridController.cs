using System;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public class GridController : IDisposable
    {
        private readonly Grid _grid;

        public Grid Grid => _grid;

        public GridController(Grid grid)
        {
            _grid = grid;
        }

        public void CenterGrid(float mapWidth, float mapHeight)
        {
            Vector3 gridSize = new Vector3(
                mapWidth * _grid.cellSize.x,
                mapWidth * _grid.cellSize.y,
                0f
            );

            _grid.transform.position =
                -gridSize * 0.5f +
                new Vector3(
                    _grid.cellSize.x * 0.5f,
                    _grid.cellSize.y * 0.5f,
                    0f
                );
        }

        public void Dispose()
        {

        }
    }
}