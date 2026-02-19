using System;

public sealed class PlacementController
{
    public event Action OnPlaced;
    public event Action OnCanceled;

    private bool _isDragging;
    private bool _enabled;

    private MapObject _draggedObject;

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
        MapManager.Instance.RemoveMapObject(_draggedObject);

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

    private void Place()
    {
        if (_draggedObject == null) return;

        var mapPos = MapUtils.WorldToMap(_draggedObject.transform.position, MapManager.Instance.Grid);
        MapManager.Instance.PlaceMapObject(mapPos, _draggedObject);
        _draggedObject.MapPos = mapPos;
        _draggedObject.transform.position = MapUtils.MapToWorld(mapPos, MapManager.Instance.Grid); 
        OnPlaced?.Invoke();
    }

    private void Cancele()
    {
        if (_draggedObject == null) return;

        var mapPos = _draggedObject.MapPos;
        _draggedObject.transform.position = MapUtils.MapToWorld(mapPos, MapManager.Instance.Grid);
        MapManager.Instance.PlaceMapObject(mapPos, _draggedObject);
        OnCanceled?.Invoke();
    }

    private bool IsValidPlacement(MapObject mapObject)
    {
        return MapManager.Instance.IsValidPlacement(mapObject);
    }
}
