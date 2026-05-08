using System;
using System.Collections.Generic;
using TToTT.TowerDefense.Map;
using UnityEngine;

public class SellectionController : IDisposable
{
    private readonly CellSelectionFactory _selectionFactory;
    private readonly DragAndDropController _dragController;
    private readonly MapController _mapController;
    private readonly GridController _gridController;

    private MapObject _selectedObject;

    private readonly List<CellSelection> _selections = new();

    #region Init

    public SellectionController(
        CellSelectionFactory selectionFactory,
        DragAndDropController dragAndDropController,
        MapController mapController,
        GridController gridController
        )
    {
        _selectionFactory = selectionFactory;
        _selectionFactory.Init();
        _dragController = dragAndDropController;
        _mapController = mapController;
        _gridController = gridController;

        Subscribe();
    }

    public void Dispose()
    {
        Unsubscride();
    }

    #endregion

    public void Tick(float dt)
    {
        if (_selectedObject != null)
        {
            ClearSellection();
            DrawObjectBorder(_selectedObject);
        }
    }

    private void OnDragStarted(MapObject dragged)
    {
        TrySelectObject(dragged);
    }

    private void OnDragCanceled(MapObject dragged)
    {
        UnselectObject(dragged);
    }

    public void DrawObjectBorder(MapObject mapObject)
    {
        var worldPos = mapObject.transform.position;
        var mapPos = MapUtils.WorldToMap(worldPos, _gridController.Grid);
        var occupiedCells = MapUtils.GetOccupiedCells(mapPos, mapObject.Shape);

        for (int i = 0; i < occupiedCells.Count; i++)
        {
            var seleciton = _selectionFactory.Create();
            seleciton.transform.position = MapUtils.MapToWorld(occupiedCells[i], _gridController.Grid);

            if (_mapController.IsCellAvailable(occupiedCells[i]))
                seleciton.SetFreeColor();
            else
                seleciton.SetBusyColor();

            seleciton.transform.rotation = Quaternion.identity;
            _selections.Add(seleciton);
        }
    }

    public void ClearSellection()
    {
        if (_selections.Count < 0) return;

        for (int i = _selections.Count - 1; i >= 0; i--)
        {
            _selectionFactory.Return(_selections[i]);
        }

        _selections.Clear();
    }

    private void TrySelectObject(MapObject obj)
    {
        _selectedObject = obj;
    }

    private void UnselectObject(MapObject obj)
    {
        if (_selectedObject == null) return;

        ClearSellection();
        _selectedObject = null;
    }

    private void Subscribe()
    {
        _dragController.OnDragStarted += OnDragStarted;
        _dragController.OnDragEnded += OnDragCanceled;
    }

    private void Unsubscride()
    {
        _dragController.OnDragStarted -= OnDragStarted;
        _dragController.OnDragEnded -= OnDragCanceled;
    }
}
