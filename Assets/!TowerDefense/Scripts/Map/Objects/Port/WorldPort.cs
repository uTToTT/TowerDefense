using UnityEngine;

public struct WorldPort
{
    public IMapObject Owner;
    public Vector2Int Cell;
    public PortDirection Direction;
    public PortType Type;
}
