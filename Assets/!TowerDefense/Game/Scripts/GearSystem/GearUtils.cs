using UnityEngine;

public static class GearUtils
{
    public static readonly PortDirection[] Directions = new PortDirection[]
    {
        PortDirection.Up,
        PortDirection.Down,
        PortDirection.Left,
        PortDirection.Right,
    };

    public static readonly Vector2Int[] Offsets = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
    };
}
