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
        _placement.BeginDrag(preview);
        OnDragStarted?.Invoke(_dragged);
    }

    public void EndDrag()
    {
        if (_dragged == null) return;
        _placement.EndDrag(_dragged);
    }

    private void HandlePlaced(Vector2Int mapPos)
    {
        var preview = _dragged;
        _dragged = null;

        if (!_buildController.TryBuild(preview.Type, mapPos, out var obj))
        {
            OnDropFailed?.Invoke(preview);
            return;
        }

        OnDropSuccess?.Invoke(preview, obj, mapPos);
    }

    private void HandleCanceled()
    {
        OnDropFailed?.Invoke(_dragged);
        _dragged = null;
    }

    public void Dispose()
    {
        _placement.OnPlaced -= HandlePlaced;
        _placement.OnCanceled -= HandleCanceled;
    }
}