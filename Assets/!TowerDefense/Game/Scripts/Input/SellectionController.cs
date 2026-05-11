using System;
using System.Collections.Generic;
using TToTT.TowerDefense.Gameloop;
using TToTT.TowerDefense.Map;
using UnityEngine;

public class SellectionController : IDisposable
{
    private const float HOLD_THRESHOLD = 0.05f;

    private readonly CellSelectionFactory _selectionFactory;
    private readonly DragAndDropController _dragController;
    private readonly MapController _mapController;
    private readonly GridController _gridController;
    private readonly GameStateMachine _gameState;
    private readonly PlayerInputController _input;

    private float _tapTimer = 0f;
    private bool _isPressing = false;
    private Vector2 _tapWorldPos;

    private MapObject _selectedObject;
    private Vector2Int _lastDrawnPos;

    private readonly List<CellSelection> _selections = new();

    #region Init

    public SellectionController(
        CellSelectionFactory selectionFactory,
        DragAndDropController dragAndDropController,
        MapController mapController,
        GridController gridController,
        GameStateMachine gameState,
        PlayerInputController input)
    {
        _selectionFactory = selectionFactory;
        _selectionFactory.Init();
        _dragController = dragAndDropController;
        _mapController = mapController;
        _gridController = gridController;
        _gameState = gameState;
        _input = input;

        Subscribe();
    }

    public void Dispose()
    {
        Unsubscride();
    }

    #endregion

    public void Tick(float dt)
    {
        // Pressing
        if (_isPressing)
        {
            _tapTimer += dt;
            if (_tapTimer >= HOLD_THRESHOLD)
            {
                _isPressing = false;
                TryBeginDragBuilt(_tapWorldPos);
            }
        }

        // Sellection draw
        if (_selectedObject == null) return;
        var worldPos = _selectedObject.transform.position;
        var mapPos = MapUtils.WorldToMap(worldPos, _gridController.Grid);
        if (mapPos == _lastDrawnPos) return;
        _lastDrawnPos = mapPos;
        ClearSellection();
        DrawObjectBorder(_selectedObject);
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
        if (_selections.Count <= 0) return;

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

    private void HandleTap(Vector2 worldPos)
    {
        if (_gameState.State != GameState.Preparing) return;
        _tapWorldPos = worldPos;
        _isPressing = true;
        _tapTimer = 0f;
    }

    private void HandleTapReleased()
    {
        _isPressing = false;
        _tapTimer = 0f;
    }

    private void EndDragOnRelease()
    {
        if (_dragController.IsDragging) 
            _dragController.EndDrag();
    }

    private void TryBeginDragBuilt(Vector2 worldPos)
    {
        var mapPos = MapUtils.WorldToMap(worldPos, _gridController.Grid);
        if (!_mapController.TryGetObject(mapPos, out var mapObject)) return;
        if (mapObject is not Tower tower) return;
        _dragController.BeginDragBuilt(tower);
    }

    private void Subscribe()
    {
        _dragController.OnDragStarted += OnDragStarted;
        _dragController.OnDragEnded += OnDragCanceled;
        _input.OnTap += HandleTap;

        _input.OnTapReleased += HandleTapReleased; 
        _input.OnTapCanceled += EndDragOnRelease;  
    }

    private void Unsubscride()
    {
        _dragController.OnDragStarted -= OnDragStarted;
        _dragController.OnDragEnded -= OnDragCanceled;
        _input.OnTap -= HandleTap;

        _input.OnTapReleased -= HandleTapReleased;
        _input.OnTapCanceled -= EndDragOnRelease;
    }
}
