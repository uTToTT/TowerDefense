using UnityEngine;

public static class MapUtils
{
    public static Vector3 GridToWorld(Vector2Int cell, Grid grid) =>
        GridToWorld(cell.x, cell.y, grid);

    public static Vector3 GridToWorld(int x, int y, Grid grid) =>
        grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
}
