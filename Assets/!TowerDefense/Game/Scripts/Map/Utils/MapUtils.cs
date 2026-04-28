using System.Collections.Generic;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public static class MapUtils
    {
        public static Vector3 SnapToGrid(Vector3 worldPos, Grid grid)
        {
            var mapPos = WorldToMap(worldPos, grid);
            var snapped = MapToWorld(mapPos, grid);
            return snapped;
        }

        /* =========================
         * MAP <-> WORLD
         * (map logic)
         * (0,0 — left down corner)
         * ========================= */

        public static Vector2Int WorldToMap(Vector3 worldPos, Grid grid)
        {
            Vector3 local = worldPos - grid.transform.position;

            int x = Mathf.FloorToInt(local.x / grid.cellSize.x);
            int y = Mathf.FloorToInt(local.y / grid.cellSize.y);

            return new Vector2Int(x, y);
        }

        public static Vector3 MapToWorld(Vector2Int mapPos, Grid grid)
        {
            return grid.transform.position +
                   new Vector3(
                       (mapPos.x + 0.5f) * grid.cellSize.x,
                       (mapPos.y + 0.5f) * grid.cellSize.y,
                       0f
                   );
        }

        /* =========================
         * GRID <-> WORLD
         * (Unity Grid)
         * ========================= */

        public static Vector3 GridToWorld(Vector2Int cell, Grid grid) =>
            grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));

        public static Vector3 GridToWorld(int x, int y, Grid grid) =>
            grid.GetCellCenterWorld(new Vector3Int(x, y, 0));

        public static Vector3Int WorldToGrid(Vector3 worldPos, Grid grid) =>
            grid.WorldToCell(worldPos);

        /* =========================
         * Cells
         * ========================= */

        public static List<Vector2Int> GetOccupiedCells(
           Vector2Int anchor,
           MapObjectShape shape)
        {
            var result = new List<Vector2Int>();

            foreach (var offset in shape.OccupiedCells)
            {
                result.Add(new Vector2Int(
                    anchor.x + offset.X,
                    anchor.y + offset.Y
                ));
            }

            return result;
        }
    }
}