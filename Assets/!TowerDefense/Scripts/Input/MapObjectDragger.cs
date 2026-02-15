using System;

public sealed class MapObjectDragger
{
    public event Action<MapObject> OnBeginDrag;
    public event Action<MapObject> OnEndDrag;

    private MapObject DragedObject;

    private bool _isDragging;
    private bool _enabled;

    public void Tick(float dt)
    {
        if (!_enabled) return;
        if (!_isDragging || DragedObject == null) return;

        MapUtils.SnapToGridUnderPointer(DragedObject.transform);
    }

    public void EnableDrag() => _enabled = true;
    public void DisableDrag() => _enabled = false;

    public void BeginDrag(MapObject obj)
    {
        if (DragedObject != null) return;

        DragedObject = obj;
        _isDragging = true;
    }

    public void EndDrag()
    {
        if (DragedObject == null) return;

        DragedObject = null;
        _isDragging = false;
    }


}
