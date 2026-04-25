using UnityEngine;

public class MapBounds
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public void SetSize(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public bool IsInside(int x, int y) =>
        x >= 0 && y >= 0 && x < Width && y < Height;

    public bool IsInside(Vector2Int pos) => IsInside(pos.x, pos.y);
}