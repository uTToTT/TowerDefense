using UnityEngine;

public interface IMapObject
{
    MapObjectType Type { get; }
    Transform Transform { get; }
    Vector2Int MapPos { get; }
    MapObjectShape Shape { get; }
}
