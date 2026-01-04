using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public Vector2Int gridPos;
    public Vector3 worldPos;
    public List<PathNode> neighbors = new();
}
