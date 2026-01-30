using UnityEngine;

public static class PortDirectionExtension 
{
    public static Vector2Int ToOffset(this PortDirection dir) =>
         dir switch
         {
             PortDirection.Up => Vector2Int.up,
             PortDirection.Down => Vector2Int.down,
             PortDirection.Left => Vector2Int.left,
             PortDirection.Right => Vector2Int.right,
             _ => Vector2Int.zero
         };

    public static PortDirection Opposite(this PortDirection dir) =>
        dir switch
        {
            PortDirection.Up => PortDirection.Down,
            PortDirection.Down => PortDirection.Up,
            PortDirection.Left => PortDirection.Right,
            PortDirection.Right => PortDirection.Left,
            _ => dir
        };
}
