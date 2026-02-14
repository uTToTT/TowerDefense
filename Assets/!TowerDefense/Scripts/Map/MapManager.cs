using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [HorizontalLine]
    [SerializeField] private bool _debug;
    [SerializeField] private bool _drawPorts;

    [HorizontalLine]
    [SerializeField] private CellSelectionFactory _selectionFactory;

    [HorizontalLine]
    [SerializeField] private Grid _grid;
    [SerializeField] private MapComposer _mapComposer;
    [SerializeField] private MapData _mapData;

    private CellData[,] _cellData;
    private List<CellSelection> _selections = new();

    private bool _isDrawMapObjectPorts;
    private MapObject _selectedMapObject;

    public Grid Grid => _grid;

    public static MapManager Instance { get; private set; }

    #region Init

    public void Init()
    {
        Instance = this;

        CenterGrid(_mapData);
        _selectionFactory.Init();
        _cellData = new CellData[_mapData.width, _mapData.height];
        _mapComposer.Build(_mapData, _cellData, _grid);
    }

    #endregion

    #region Unity API

    private void OnDrawGizmos()
    {
        if (!_drawPorts) return;
        if (_isDrawMapObjectPorts &&
            _selectedMapObject.Shape.Ports != null &&
            _selectedMapObject.Shape.Ports.Length > 0)
        {
            DrawPorts(_selectedMapObject);
        }
    }

    #endregion

    public Route GetRoute(RouteId routeId) =>
        _mapData.GetRoute(routeId);

    public CellData GetCellData(Vector2Int v2Int) =>
        IsInside(v2Int) ? _cellData[v2Int.x, v2Int.y] : null;

    public bool IsInside(Vector2Int pos) =>
        pos.x >= 0 &&
        pos.y >= 0 &&
        pos.x < _mapData.width &&
        pos.y < _mapData.height;

    public bool IsCellBusy(Vector2Int pos)
    {
        var cell = GetCellData(pos);
        return cell != null && cell.IsBusy;
    }

    public void PlaceMapObject(Vector2Int pos, MapObject mapObject)
    {
        var cell = GetCellData(pos);
        if (cell != null && mapObject != null)
        {
            cell.MapObject = mapObject;
            cell.IsBusy = true;
        }
    }

    public void RemoveMapObject(Vector2Int pos)
    {
        var cell = GetCellData(pos);
        if (cell != null)
        {
            cell.MapObject = null;
            cell.IsBusy = false;
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

    public CellData Raycast()
    {
        if (GameManager.Instance.PlayerInputController.IsPointerOverUI())
            return null;

        var worldPos = GameManager.Instance.PlayerInputController.GetPointerPosition();
        var mapPos = MapUtils.WorldToMap(worldPos, Grid);

        return GetCellData(mapPos);
    }

    public void DrawBorderMapObject(MapObject mapObject)
    {
        var occupiedCells = MapUtils.GetOccupiedCells(mapObject.MapPos, mapObject.Shape);

        for (int i = 0; i < occupiedCells.Count; i++)
        {
            var seleciton = _selectionFactory.Create();
            seleciton.transform.position = MapUtils.MapToWorld(occupiedCells[i], Grid);
            seleciton.transform.rotation = Quaternion.identity;
            seleciton.transform.parent = mapObject.transform;
            _selections.Add(seleciton);
        }
    }

    public bool HasMapObject(Vector2Int pos, MapObjectType type, out MapObject mapObject)
    {
        var cell = GetCellData(pos);

        var hasObject =
            cell != null &&
            cell.MapObject != null &&
            cell.MapObject.Type == type;

        mapObject = hasObject ? cell.MapObject : null;

        return hasObject;
    }

    #region Ports

    public void ShowMapObjectPorts(MapObject mapObject)
    {
        _isDrawMapObjectPorts = true;
        _selectedMapObject = mapObject;
    }

    public void HideMapObjectPorts()
    {
        _isDrawMapObjectPorts = false;
        _selectedMapObject = null;
    }

    public static List<WorldPort> GetWorldPorts(MapObject obj)
    {
        var result = new List<WorldPort>();

        foreach (var port in obj.Shape.Ports)
        {
            var worldCell = new Vector2Int(
                obj.MapPos.x + port.Cell.X,
                obj.MapPos.y + port.Cell.Y
            );

            result.Add(new WorldPort
            {
                Owner = obj,
                Cell = worldCell,
                Direction = port.Direction,
                Type = port.Type
            });
        }

        return result;
    }

    public void ResolveConnections(MapObject placedObject)
    {
        var ports = GetWorldPorts(placedObject);

        foreach (var port in ports)
        {
            var targetCell = port.Cell + port.Direction.ToOffset();

            var cellData = GetCellData(targetCell);
            if (cellData?.MapObject == null ||
                cellData?.MapObject is not MapObject otherObject)
                continue;

            var otherPorts = GetWorldPorts(otherObject);

            foreach (var otherPort in otherPorts)
            {
                if (MapUtils.ArePortsConnected(port, otherPort))
                {
                    Debug.Log($"[{port.Cell}]&[{otherPort.Cell}] | [{port.Type}] Connected");
                    //ApplyBuff(port, otherPort);
                }
            }
        }
    }

    public static List<KeyValuePair<Vector2Int, PortDirection>> GetPortCells(
       Vector2Int anchor,
       MapObjectShape shape)
    {
        var result = new List<KeyValuePair<Vector2Int, PortDirection>>();

        foreach (var offset in shape.Ports)
        {
            var cell = new Vector2Int(
                anchor.x + offset.Cell.X,
                anchor.y + offset.Cell.Y);

            result.Add(new KeyValuePair<Vector2Int, PortDirection>(
                cell,
                offset.Direction));
        }

        return result;
    }

    private void DrawPorts(MapObject mapObject)
    {
        var ports = GetPortCells(mapObject.MapPos, mapObject.Shape);

        for (int i = 0; i < ports.Count; i++)
        {
            var portOrigin = MapUtils.MapToWorld(ports[i].Key, Grid);
            var portEnd = MapUtils.MapToWorld(ports[i].Key + ports[i].Value.ToOffset(), Grid);

            Debug.DrawLine(portOrigin, portEnd, Color.red);
        }
    }

    #endregion

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
}
