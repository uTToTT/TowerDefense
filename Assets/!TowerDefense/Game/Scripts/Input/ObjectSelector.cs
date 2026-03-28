using System;
using System.Collections.Generic;
using TToTT.TowerDefense.Map;
using UnityEngine;

public class ObjectSelector : IDisposable
{
    public event Action<MapObject> OnObjectSelected;

    private readonly MapManager _mapManager;
    private readonly CellSelectionFactory _selectionFactory; // relocate to ObjectSelectorView
    private readonly PlayerInputController _playerInputController; // change to interface

    private MapObject _selectedObject;

    private readonly List<CellSelection> _selections = new(); 

    #region Init

    public ObjectSelector(
        PlayerInputController playerInputController,
        MapManager mapManager,
        CellSelectionFactory selectionFactory)
    {
        _mapManager = mapManager;
        _playerInputController = playerInputController;
        _selectionFactory = selectionFactory;

        _playerInputController.OnTapPerformed += OnTapPerformed;
        _playerInputController.OnTapCanceled += OnTapCanceled;
    }

    public void Dispose()
    {
        Unsubscride();
    }

    #endregion

    public void Tick(float dt)
    {
        //if(_selectedObject != null)
        //{
        //    MapManager.Instance.ClearSellection();
        //    MapManager.Instance.DrawBorderMapObject(_selectedObject);
        //}
    }

    private void OnTapPerformed()
    {
        var cell = _mapManager.Raycast();

        TrySelectObject(cell);
    }

    private void OnTapCanceled()
    {
        UnselectObject();
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

    private void TrySelectObject(CellData cell)
    {
        if (_mapManager.TryGetObject(cell, out var mapObject))
        {
            _selectedObject = mapObject;

            //MapManager.Instance.ClearSellection();
            //MapManager.Instance.DrawBorderMapObject(_selectedObject);
            MapManager.Instance.PlacementController.BeginDrag(_selectedObject);
        }
    }

    private void UnselectObject()
    {
        if (_selectedObject == null) return;

        MapManager.Instance.ClearSellection();
        MapManager.Instance.PlacementController.EndDrag(_selectedObject);

        _selectedObject = null;
    }

    private void Subscribe()
    {
        _playerInputController.OnTapPerformed += OnTapPerformed;
        _playerInputController.OnTapCanceled += OnTapCanceled;
    }

    private void Unsubscride()
    {
        _playerInputController.OnTapPerformed -= OnTapPerformed;
        _playerInputController.OnTapCanceled -= OnTapCanceled;
    }

    public void DrawBorderMapObject(MapObject mapObject)
    {
        var worldPos = mapObject.transform.position;
        var mapPos = MapUtils.WorldToMap(worldPos, _grid);
        var occupiedCells = MapUtils.GetOccupiedCells(mapPos, mapObject.Shape);

        for (int i = 0; i < occupiedCells.Count; i++)
        {
            var seleciton = _selectionFactory.Create();
            seleciton.transform.position = MapUtils.MapToWorld(occupiedCells[i], Grid);

            if (IsCellBusy(occupiedCells[i]))
                seleciton.SetBusyColor();
            else
                seleciton.SetFreeColor();

            seleciton.transform.rotation = Quaternion.identity;
            _selections.Add(seleciton);
        }
    }
}
