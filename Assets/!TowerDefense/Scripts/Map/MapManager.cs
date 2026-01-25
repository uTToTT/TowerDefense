using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private bool _debug;
    [HorizontalLine]

    [SerializeField] private Grid _grid;
    [SerializeField] private MapComposer _mapComposer;
    [SerializeField] private MapData _mapData;

    private CellData[,] _cellData;

    public Grid Grid => _grid;

    public static MapManager Instance { get; private set; }

    public void Init()
    {
        Instance = this;

        CenterGrid(_mapData);
        _cellData = new CellData[_mapData.width, _mapData.height];
        _mapComposer.Build(_mapData, _cellData, _grid);
    }

    public Route GetRoute(RouteId routeId) =>
     _mapData.GetRoute(routeId);

    public CellData GetCellData(Vector2Int v2Int) =>
        _cellData[v2Int.x, v2Int.y];

    public bool IsInside(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 &&
               pos.x < _mapData.width &&
               pos.y < _mapData.height;
    }

    public bool IsInside(Vector3 worldPos)
    {
        var mapPos = MapUtils.WorldToMap(worldPos, _grid);
        var isInside = IsInside(mapPos);

        if (_debug)
        {
            Debug.Log($"{mapPos} is inside: [{isInside}]");
        }

        return isInside;
    }

    public bool IsCellBusy(Vector2Int pos)
    {
        var isBusy = _cellData[pos.x, pos.y].IsBusy;

        if (_debug)
        {
            Debug.Log($"{pos} is busy: [{isBusy}]");
        }

        return isBusy;
    }

    public void SetBusyState(Vector2Int pos, bool state) =>
        _cellData[pos.x, pos.y].IsBusy = state;

    public void SetTowerInCell(Vector2Int pos, Tower tower)
    {
        if (tower != null)
        {
            _cellData[pos.x, pos.y].MapObject = tower;
            _cellData[pos.x, pos.y].IsBusy = true;
        }
    }

    public List<Vector3> GetRoutePoints(RouteId routeId)
    {
        var points = new List<Vector3>();
        var route = GetRoute(routeId);

        foreach (var point in route.points)
        {
            points.Add(MapUtils.GridToWorld(point, _grid));
        }

        return points;
    }

    private void CenterGrid(MapData map)
    {
        Vector3 gridSize = new Vector3(
            map.width * _grid.cellSize.x,
            map.height * _grid.cellSize.y,
            0f
        );

        _grid.transform.position =
            -gridSize * 0.5f +
            new Vector3(
                _grid.cellSize.x * 0.5f,
                _grid.cellSize.y * 0.5f,
                0f
            );
    }
}
