using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapComposer _mapComposer;
    [SerializeField] private MapData _mapData;

    private PathGraph _pathGraph;
    private List<PathNode> _path;
    private List<Vector3> _points;

    private void Start()
    {
        _points = new List<Vector3>();

        _mapComposer.Build(_mapData);

        _pathGraph = PathGraphBuilder.Build(_mapData, _mapData.cellSize);
        _path = BFS.FindPath(
            _pathGraph.entrances[0],
            _pathGraph.exits[0]);

        foreach (var node in _path)
            _points.Add(node.worldPos);
    }

    private void OnDrawGizmosSelected()
    {
        if (_points == null || _points.Count < 2)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < _points.Count - 1; i++)
        {
            Gizmos.DrawLine(_points[i], _points[i + 1]);
            Gizmos.DrawSphere(_points[i], 0.1f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_points[^1], 0.12f);
    }
}
