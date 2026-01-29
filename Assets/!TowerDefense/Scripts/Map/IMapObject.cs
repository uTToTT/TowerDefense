using UnityEngine;

public interface IMapObject
{
    Transform Transform { get; }
    Vector2Int MapPos { get; }
    MapObjectShape Shape { get; }
}
