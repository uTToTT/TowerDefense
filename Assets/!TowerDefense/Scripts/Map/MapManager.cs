using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [HorizontalLine]
    [SerializeField] private bool _debug;

    [HorizontalLine]
    [SerializeField] private CellSelectionFactory _selectionFactory;

    [HorizontalLine]
    [SerializeField] private Grid _grid;
    [SerializeField] private MapComposer _mapComposer;
    [SerializeField] private MapData _mapData;

    private CellData[,] _cellData;
    private List<CellSelection> _selections = new();

    public Grid Grid => _grid;

    public static MapManager Instance { get; private set; }

    public void Init()
    {
        Instance = this;

        CenterGrid(_mapData);
        _selectionFactory.Init();
        _cellData = new CellData[_mapData.width, _mapData.height];
        _mapComposer.Build(_mapData, _cellData, _grid);
    }

    public Route GetRoute(RouteId routeId) =>
     _mapData.GetRoute(routeId);

    public CellData GetCellData(Vector2Int v2Int)
    {
        if (IsInside(v2Int))
            return _cellData[v2Int.x, v2Int.y];

        return null;
    }

    public bool IsInside(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 &&
               pos.x < _mapData.width &&
               pos.y < _mapData.height;
    }

    public bool IsInside(Vector3 worldPos)
    {
        var mapPos = MapUtils.WorldToMap(worldPos, _grid);
        return IsInside(mapPos);
    }

    public bool IsCellBusy(Vector2Int pos)
    {
        if (IsInside(pos))
            return _cellData[pos.x, pos.y].IsBusy;

        return false;
    }

    public void SetBusyState(Vector2Int pos, bool state)
    {
        if (IsInside(pos))
            _cellData[pos.x, pos.y].IsBusy = state;
    }

    public void SetTowerInCell(Vector2Int pos, Tower tower)
    {
        if (tower != null)
        {
            _cellData[pos.x, pos.y].MapObject = tower;
            _cellData[pos.x, pos.y].IsBusy = true;
        }
    }

    public void RemoveTowerInCell(Vector2Int pos)
    {
        _cellData[pos.x, pos.y].MapObject = null;
        _cellData[pos.x, pos.y].IsBusy = false;
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

    public CellData Raycast()
    {
        if (GameManager.Instance.PlayerInputController.IsPointerOverUI())
            return null;

        var worldPos = GameManager.Instance.PlayerInputController.GetPointerPosition();
        var mapPos = MapUtils.WorldToMap(worldPos, Grid);

        return GetCellData(mapPos);
    }

    public void DrawBorderMapObject(IMapObject mapObject)
    {
        var occupiedCells = GetOccupiedCells(mapObject.MapPos, mapObject.Shape);

        for (int i = 0; i < occupiedCells.Count; i++)
        {
            var seleciton = _selectionFactory.Create();
            seleciton.transform.position = MapUtils.MapToWorld(occupiedCells[i], Grid);
            seleciton.transform.rotation = Quaternion.identity;
            seleciton.transform.parent = mapObject.Transform;
            _selections.Add(seleciton);
        }
    }

    public void ClearSellection()
    {
        if (_selections.Count < 0) return;

        for (int i = _selections.Count - 1; i >= 0; i--)
        {
            _selectionFactory.Return(_selections[i]);
        }

        _selections.Clear();
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

    public static List<Vector2Int> GetOccupiedCells(
    Vector2Int anchor,
    MapObjectShape shape)
    {
        var result = new List<Vector2Int>();

        foreach (var offset in shape.OccupiedCells)
        {
            result.Add(new Vector2Int(
                anchor.x + offset.X,
                anchor.y + offset.Y
            ));
        }

        return result;
    }
}
