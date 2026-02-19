using System;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    [SerializeField] private CellSelectionFactory _selectionFactory;

    public event Action<MapObject> OnObjectSelected;

    private MapManager _mapManager;
    private MapObject _selectedObject;

    public void Init(PlayerInputController playerInputController, MapManager mapManager)
    {
        _mapManager = mapManager;

        playerInputController.OnTapPerformed += OnTapPerformed;
        playerInputController.OnTapCanceled += OnTapCanceled;

        MapManager.Instance.ClearSellection();
    }

    public void Tick(float dt)
    {
        if(_selectedObject != null)
        {
            MapManager.Instance.ClearSellection();
            MapManager.Instance.DrawBorderMapObject(_selectedObject);
        }
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
}
