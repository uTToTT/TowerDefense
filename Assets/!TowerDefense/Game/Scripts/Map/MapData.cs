using System.Collections.Generic;
using UnityEngine;

public class MapData : ScriptableObject
{
    public int buildableCount;
    public int blockedCount;
    public int pathCount;
    public int entranceCount;
    public int exitCount;

    public float cellSize = 2;
    public int width;
    public int height;
    public int size;

    public CellType[] cells;
    public List<Route> routes;

    public CellType Get(int x, int y) =>
        cells[y * width + x];

    public bool IsInside(int x, int y) =>
        x >= 0 && y >= 0 &&
        x < width && y < height;

    public bool IsInside(Vector2Int v) => IsInside(v.x, v.y);
}
