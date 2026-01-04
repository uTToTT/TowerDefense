using System.Collections.Generic;
using UnityEngine;

public class PathGraph
{
    public Dictionary<Vector2Int, PathNode> nodes = new();

    public List<PathNode> entrances = new();
    public List<PathNode> exits = new();
}
