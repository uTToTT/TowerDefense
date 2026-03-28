using System;
using TToTT.TowerDefense.Map;
using UnityEngine;

public sealed class PlacementController : IDisposable
{
    public event Action OnPlaced;
    public event Action OnCanceled;

    private readonly GridController _gridController;

    private bool _isDragging;
    private bool _enabled;

    private MapObject _draggedObject;

    #region Init

    public PlacementController(GridController gridController)
    {
        _gridController = gridController;
    }

    public void Dispose()
    {

    }

    #endregion

    public void Tick(float dt)
    {
        if (!_enabled) return;
        if (!_isDragging) return;

        if (_draggedObject != null)
        {
            DragUtils.SnapToPointer(_draggedObject.transform);
            MapManager.Instance.ClearSellection();
            MapManager.Instance.DrawBorderMapObject(_draggedObject);
        }
    }

    public void EnableDrag() => _enabled = true;
    public void DisableDrag() => _enabled = false;

    public void BeginDrag(MapObject mapObject)
    {
        _draggedObject = mapObject;

        if (mapObject is Tower tower)
        {
            tower.Disable();
            tower.ShowRange();
        }

        MapManager.Instance.RemoveMapObject(_draggedObject);

        _isDragging = true;
    }

    public void EndDrag(MapObject mapObject)
    {
        if (IsValidPlacement(mapObject))
            Place();
        else
            Cancele();

        if (mapObject is Tower tower)
        {
            tower.Enable();
            tower.HideRange();
        }

        _draggedObject = null;

        _isDragging = false;
    }

    private void Place()
    {
        if (_draggedObject == null) return;

        var mapPos = MapUtils.WorldToMap(_draggedObject.transform.position, MapManager.Instance.Grid);
        MapManager.Instance.PlaceMapObject(mapPos, _draggedObject);
        _draggedObject.MapPos = mapPos;
        _draggedObject.transform.position = MapUtils.MapToWorld(mapPos, MapManager.Instance.Grid);

        MapManager.Instance.ClearSellection();
        OnPlaced?.Invoke();
    }

    private void Cancele()
    {
        if (_draggedObject == null) return;

        var mapPos = _draggedObject.MapPos;
        _draggedObject.transform.position = MapUtils.MapToWorld(mapPos, _gridController.Grid);

        MapManager.Instance.PlaceMapObject(mapPos, _draggedObject);
        MapManager.Instance.ClearSellection();
        OnCanceled?.Invoke();
    }

    public bool IsValidPlacement(MapObject mapObject)
    {
        Vector2Int mapPos = MapUtils.WorldToMap(mapObject.transform.position, _gridController.Grid);

        foreach (var cell in MapUtils.GetOccupiedCells(mapPos, mapObject.Shape))
        {
            if (!IsInside(cell))
                return false;
            if (IsCellBusy(cell))
                return false;
        }

        return true;
    }

    
}
