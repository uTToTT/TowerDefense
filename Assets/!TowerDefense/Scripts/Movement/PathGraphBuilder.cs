using UnityEngine;

public static class PathGraphBuilder
{
    private static readonly Vector2Int[] Directions =
    {
        Vector2Int.right,
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.down
    };

    public static PathGraph Build(MapData map, float cellSize)
    {
        var graph = new PathGraph();

        for (int y = 0; y < map.height; y++)
        {
            for (int x = 0; x < map.width; x++)
            {
                var type = map.Get(x, y);
                if (type is not (CellType.Path or CellType.Entrance or CellType.Exit))
                    continue;

                var node = new PathNode
                {
                    gridPos = new Vector2Int(x, y),
                    worldPos = MapUtils.GridToWorld(x,y,map)                      
                };

                graph.nodes[node.gridPos] = node;

                if (type == CellType.Entrance)
                {
                    graph.entrances.Add(node);
                }
                else if (type == CellType.Exit)
                {
                    graph.exits.Add(node);
                }
            }
        }
            

        foreach (var node in graph.nodes.Values)
        {
            foreach (var d in Directions)
            {
                var p = node.gridPos + d;
                if (graph.nodes.TryGetValue(p, out var neighbor))
                    node.neighbors.Add(neighbor);
            }
        }

        return graph;
    }
}
