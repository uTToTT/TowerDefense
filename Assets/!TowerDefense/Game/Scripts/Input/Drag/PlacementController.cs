using System;
using TToTT.TowerDefense.Map;
using UnityEngine;

public sealed class PlacementController : IDisposable
{
    public event Action<Vector2Int> OnPlaced;
    public event Action OnCanceled;

    private readonly GridController _gridController;
    private bool _isDragging;
    private bool _enabled;
    private MapObject _draggedObject;

    public Grid Grid => _gridController.Grid;

    public PlacementController(GridController gridController)
    {
        _gridController = gridController;
    }

    public void Tick(float dt)
    {
        if (!_enabled || !_isDragging) return;
        if (_draggedObject != null)
            DragUtils.SnapToPointer(_draggedObject.transform);
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
        _isDragging = true;
    }

    public void EndDrag(MapObject mapObject)
    {
        var mapPos = MapUtils.WorldToMap(
            _draggedObject.transform.position,
            _gridController.Grid);

        _draggedObject.transform.position =
            MapUtils.MapToWorld(mapPos, _gridController.Grid);

        if (mapObject is Tower tower)
        {
            tower.Enable();
            tower.HideRange();
        }

        _draggedObject = null;
        _isDragging = false;

        OnPlaced?.Invoke(mapPos);
    }

    public void Cancel()
    {
        _draggedObject = null;
        _isDragging = false;
        OnCanceled?.Invoke();
    }

    public void Dispose() { }
}