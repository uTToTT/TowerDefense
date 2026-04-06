using System;
using TToTT.TowerDefense.Map;

public sealed class PlacementController : IDisposable
{
    public event Action OnPlaced;
    public event Action OnCanceled;

    private readonly GridController _gridController;
    private readonly MapController _mapController;

    private bool _isDragging;
    private bool _enabled;

    private MapObject _draggedObject;

    #region Init

    public PlacementController(
        GridController gridController,
        MapController mapController)
    {
        _gridController = gridController;
        _mapController = mapController;
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

        _mapController.RemoveMapObject(_draggedObject);

        _isDragging = true;
    }

    public void EndDrag(MapObject mapObject)
    {
        var mapPos = MapUtils.WorldToMap(_draggedObject.transform.position, _gridController.Grid);
        bool success = _mapController.TryPlaceObject(mapPos, mapObject);

        if (success)
        {
            _draggedObject.MapPos = mapPos;
            _draggedObject.transform.position = MapUtils.MapToWorld(mapPos, _gridController.Grid);

            OnPlaced?.Invoke();
        }
        else
        {
            OnCanceled?.Invoke();
        }

        if (mapObject is Tower tower) // refactor
        {
            tower.Enable();
            tower.HideRange();
        }

        _draggedObject = null;

        _isDragging = false;
    }
}
