using System;
using TToTT.TowerDefense.Map;
using UnityEngine;

public class DragAndDropController : IDisposable
{
    public event Action<MapObject> OnDragStarted;
    public event Action<MapObject> OnDragEnded;

    public event Action<MapObject, MapObject, Vector2Int> OnDropSuccess;
    public event Action<MapObject> OnDropFailed;

    private readonly PlacementController _placement;
    private readonly BuildController _buildController;

    private MapObject _dragged;
    private bool _isMovingBuilt = false;
    private Vector2Int _previousMapPos;

    public bool IsDragging => _dragged != null;

    public DragAndDropController(
        PlacementController placement,
        BuildController buildController)
    {
        _placement = placement;
        _buildController = buildController;
        _placement.OnPlaced += HandlePlaced;
        _placement.OnCanceled += HandleCanceled;
    }

    public void BeginDrag(MapObject preview)
    {
        _dragged = preview;

        if (_dragged is Tower tower)
        {
            tower.ShowRange();
        }

        if (_dragged is TowerPreview towerPreview)
        {
            towerPreview.Enable();
        }

        _placement.BeginDrag(preview);
        OnDragStarted?.Invoke(_dragged);
    }

    public void BeginDragBuilt(Tower tower)
    {
        _previousMapPos = tower.MapPos;
        _isMovingBuilt = true;
        _dragged = tower;
        tower.ShowRange();
        tower.Disable();
        _buildController.Detach(tower); 
        _placement.BeginDrag(tower);
        OnDragStarted?.Invoke(tower);
    }

    public void EndDrag()
    {
        if (_dragged == null) return;

        if (_dragged is Tower tower)
        {
            tower.HideRange();
        }

        if (_dragged is TowerPreview towerPreview)
        {
            towerPreview.Disable();
        }

        _placement.EndDrag(_dragged);
        OnDragEnded?.Invoke(_dragged);
    }

    private void HandlePlaced(Vector2Int mapPos)
    {
        var preview = _dragged;
        _dragged = null;

        if (_isMovingBuilt)
        {
            _isMovingBuilt = false;

            if (!_buildController.TryPlace(preview, mapPos))
            {
                _buildController.TryPlace(preview, _previousMapPos);

                if (preview is Tower t) t.Enable();

                OnDropFailed?.Invoke(preview);
                return;
            }

            if (preview is Tower tower) tower.Enable(); 

            OnDropSuccess?.Invoke(preview, preview, mapPos);
            return;
        }

        if (!_buildController.TryBuild(preview.Type, mapPos, out var obj))
        {
            OnDropFailed?.Invoke(preview);
            return;
        }

        OnDropSuccess?.Invoke(preview, obj, mapPos);
        OnDragEnded?.Invoke(preview);
    }

    private void HandleCanceled()
    {
        if (_isMovingBuilt)
        {
            _isMovingBuilt = false;
            _buildController.TryPlace(_dragged, _previousMapPos);
        }

        OnDropFailed?.Invoke(_dragged);
        OnDragEnded?.Invoke(_dragged);
        _dragged = null;
    }

    public void Dispose()
    {
        _placement.OnPlaced -= HandlePlaced;
        _placement.OnCanceled -= HandleCanceled;
    }
}