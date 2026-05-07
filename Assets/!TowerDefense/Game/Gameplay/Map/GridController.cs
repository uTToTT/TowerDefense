using System;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public class GridController : IDisposable
    {
        private readonly Grid _grid;
        private readonly MapBounds _bounds;

        public Grid Grid => _grid;

        public GridController(
            Grid grid,
            MapBounds bounds)
        {
            _grid = grid;
            _bounds = bounds;
        }

        public void CenterGrid()
        {
            Vector3 gridSize = new Vector3(
                _bounds.Width * _grid.cellSize.x,
                _bounds.Height * _grid.cellSize.y,
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