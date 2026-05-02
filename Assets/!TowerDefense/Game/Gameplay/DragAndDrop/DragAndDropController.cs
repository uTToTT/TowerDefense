using System;
using TToTT.TowerDefense.Map;
using UnityEngine;

public class DragAndDropController : IDisposable
{
    public event Action<MapObject, Vector2Int> OnDropSuccess;
    public event Action<MapObject> OnDropFailed;

    private readonly PlacementController _placement;
    private MapObject _dragged;

    public DragAndDropController(PlacementController placement)
    {
        _placement = placement;
        _placement.OnPlaced += HandlePlaced;
        _placement.OnCanceled += HandleCanceled;
    }

    public void BeginDrag(MapObject preview)
    {
        _dragged = preview;
        _placement.BeginDrag(preview);
    }

    public void EndDrag()
    {
        if (_dragged == null) return;
        _placement.EndDrag(_dragged);
    }

    private void HandlePlaced()
    {
        var pos = MapUtils.WorldToMap(
            _dragged.transform.position,
            _placement.Grid);
        OnDropSuccess?.Invoke(_dragged, pos);
        _dragged = null;
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