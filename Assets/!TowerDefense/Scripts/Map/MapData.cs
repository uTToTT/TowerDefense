using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Map Data")]
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
    public CellType[] cells;

    public List<Route> routes;
    //public List<Vector2Int> entrances;
    //public List<Vector2Int> exits;

    public CellType Get(int x, int y) =>
        cells[y * width + x];

    public bool IsInside(int x, int y) =>
        x >= 0 && y >= 0 &&
        x < width && y < height;
}
