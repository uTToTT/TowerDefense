using UnityEngine;

public struct WorldPort
{
    public MapObject Owner;
    public Vector2Int Cell;
    public PortDirection Direction;
    public PortType Type;
}
