using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Map Data")]
public class MapData : ScriptableObject
{
    public float cellSize = 2;
    public int width;
    public int height;
    public CellType[] cells;

    public List<FlowData>[] flows;

    public CellType Get(int x, int y) =>
        cells[y * width + x];

    public bool IsInside(int x, int y) =>
        x >= 0 && y >= 0 && 
        x < width && y < height;

    public List<FlowData> GetFlows(int x, int y) => flows[y * width + x];
}
