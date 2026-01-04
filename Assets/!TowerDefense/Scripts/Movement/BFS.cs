using System.Collections.Generic;

public static class BFS
{
    public static List<PathNode> FindPath(
     PathNode start,
     PathNode goal)
    {
        var queue = new Queue<PathNode>();
        var cameFrom = new Dictionary<PathNode, PathNode>();

        queue.Enqueue(start);
        cameFrom[start] = null;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == goal)
                break;

            foreach (var next in current.neighbors)
            {
                if (cameFrom.ContainsKey(next))
                    continue;

                cameFrom[next] = current;
                queue.Enqueue(next);
            }
        }

        var path = new List<PathNode>();
        var c = goal;

        while (c != null)
        {
            path.Add(c);
            c = cameFrom[c];
        }

        path.Reverse();
        return path;
    }
}
