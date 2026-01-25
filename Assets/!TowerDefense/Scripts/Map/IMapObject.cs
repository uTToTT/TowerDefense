using UnityEngine;

public interface IMapObject
{
    Vector2Int Anchor { get; }
    MapObjectShape Shape { get; }
}
