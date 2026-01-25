using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private TowerBuildButton[] _buildButtons;
    [SerializeField] private Grid _grid;
    [SerializeField] private MapManager _mapManager;
    [HorizontalLine]

    [SerializeField] private TowerFactoryRegistry _towerFactory;
    [SerializeField] private TowerPreviewFactoryRegistry _towerPreviewFactory;

    private TowerPreview _towerPreview;

    private TowerType _draggingType;
    private bool _isDragging;

    public void Init()
    {
        foreach (var button in _buildButtons)
        {
            button.TowerPlacer = this;
        }
        _towerFactory.Init();
        _towerPreviewFactory.Init();
    }

    private void Update()
    {
        if (!_isDragging)
            return;

        UpdatePreviewPosition();

        if (Input.GetMouseButtonUp(0))
            TryPlaceTower();
    }

    public void BeginDrag(TowerType towerType)
    {
        if (_isDragging)
            return;

        _draggingType = towerType;
        _towerPreview = _towerPreviewFactory.Create(towerType);
        _isDragging = true;
    }

    private void UpdatePreviewPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = -Camera.main.transform.position.z;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        if (!IsValidPlacement(worldPos))
        {
            return;
        }

        _towerPreview.transform.position =
            MapUtils.SnapToGrid(worldPos, _grid);
    }

    private void TryPlaceTower()
    {
        _isDragging = false;

        var snapped = MapUtils.SnapToGrid(_towerPreview.transform.position, _grid);

        if (!IsValidPlacement(snapped))
        {
            CancelPlacement();
            return;
        }

        var tower = _towerFactory.Create(_draggingType);
        tower.Initialize(_grid);
        tower.UpgradeController.Purchase(tower.UpgradeTree);

        var mapPos = MapUtils.WorldToMap(snapped, _grid);


        foreach (var cell in CellSelector.GetOccupiedCells(mapPos, tower.Shape))
        {
            _mapManager.SetTowerInCell(cell, tower);
        }

        tower.transform.position = _towerPreview.transform.position;

        _towerPreviewFactory.Return(_towerPreview);
    }

    private void CancelPlacement()
    {
        _towerPreviewFactory.Return(_towerPreview);
    }

    private bool IsValidPlacement(Vector3 worldPos)
    {
        Vector2Int mapPos = MapUtils.WorldToMap(worldPos, _grid);

        foreach (var cell in CellSelector.GetOccupiedCells(mapPos, _towerPreview.Shape))
        {
            if (!_mapManager.IsInside(cell))
                return false;
            if (_mapManager.IsCellBusy(cell))
                return false;
        }

        return true;
    }


}
