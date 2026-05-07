using System;
using TToTT.TowerDefense.Map;
using UnityEngine;

public sealed class PlacementController : IDisposable
{
    public event Action<Vector2Int> OnPlaced;
    public event Action OnCanceled;

    private readonly GridController _gridController;
    private readonly PlayerInputController _playerInputController;
    private readonly MapController _mapController;

    private bool _isDragging;
    private MapObject _draggedObject;

    public Grid Grid => _gridController.Grid;

    public PlacementController(
        GridController gridController,
        PlayerInputController playerInputController,
        MapController mapController)
    {
        _gridController = gridController;
        _playerInputController = playerInputController;
        _mapController = mapController;
    }

    public void Tick(float dt)
    {
        if (!_isDragging) return;
        if (_draggedObject != null)
            SnapToPointer(_draggedObject.transform);
    }

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

        if (!_mapController.IsAreaAvailable(mapPos, mapObject.Shape))
        {
            Cancel();
            return;
        }

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

    private void SnapToPointer(Transform transform)
    {
        var worldPos = _playerInputController.GetPointerPosition();
        transform.position = worldPos;
    }

    public void Dispose() { }
}