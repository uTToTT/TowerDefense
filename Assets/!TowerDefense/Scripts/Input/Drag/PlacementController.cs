using System;
using UnityEngine;

public sealed class PlacementController
{
    public event Action OnPlaced;
    public event Action OnCanceled;

    private bool _isDragging;
    private bool _enabled;

    private MapObject _draggedObject;

    private Vector3 GetPointerPosition() =>
        GameManager.Instance.PlayerInputController.GetPointerPosition();

    public void Tick(float dt)
    {
        if (!_enabled) return;
        if (!_isDragging) return;

        if (_draggedObject != null)
            DragUtils.SnapToPointer(_draggedObject.transform);
    }

    public void EnableDrag() => _enabled = true;
    public void DisableDrag() => _enabled = false;

    public void BeginDrag(MapObject mapObject)
    {
        _draggedObject = mapObject;

        _isDragging = true;
    }

    public void EndDrag(MapObject mapObject)
    {
        if (IsValidPlacement(mapObject))
            Place();
        else
            Cancele();

        _draggedObject = null;

        _isDragging = false;
    }

    private void Place() => OnPlaced?.Invoke();
    private void Cancele() => OnCanceled?.Invoke();

    private bool IsValidPlacement(MapObject mapObject)
    {
        return MapManager.Instance.IsValidPlacement(mapObject, mapObject.transform.position);
    }
}
