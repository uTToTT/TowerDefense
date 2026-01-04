using UnityEngine;

[CreateAssetMenu(menuName = "Map/Map Data")]
public class MapData : ScriptableObject
{
    public int width;
    public int height;
    public CellType[] cells;

    public CellType Get(int x, int y)
        => cells[y * width + x];
}
