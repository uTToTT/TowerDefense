using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private TowerBuildButton[] _buildButtons;
    [HorizontalLine]

    [SerializeField] private TowerFactoryRegistry _towerFactory;
    [SerializeField] private TowerPreviewFactoryRegistry _towerPreviewFactory;

    private List<Tower> _builtTowers = new();
    private TowerPreview _towerPreview;

    private TowerType _draggingType;
    private bool _isDragging;

    private Grid Grid => MapManager.Instance.Grid;
    private MapManager MapManager => MapManager.Instance;

    public void Init()
    {
        foreach (var button in _buildButtons)
        {
            button.TowerPlacer = this;
        }

        _towerFactory.Init();
        _towerPreviewFactory.Init();
    }

    public void Tick(float dt)
    {
        foreach(var tower in _builtTowers)
            tower.Tick(dt);

        if (!_isDragging)
            return;

        UpdatePreviewPosition();

        if (GameManager.Instance.PlayerInputController.IsPointerDown == false)
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
        var worldPos = GameManager.Instance.PlayerInputController.GetPointerPosition();

        if (!IsValidPlacement(worldPos))
        {
            return;
        }

        _towerPreview.transform.position =
            MapUtils.SnapToGrid(worldPos, Grid);
    }

    private void TryPlaceTower()
    {
        _isDragging = false;

        var snapped = MapUtils.SnapToGrid(_towerPreview.transform.position, Grid);

        if (!IsValidPlacement(snapped))
        {
            CancelPlacement();
            return;
        }

        var tower = _towerFactory.Create(_draggingType);
        tower.transform.position = _towerPreview.transform.position;
        tower.UpgradeController.Purchase(tower.UpgradeTree);

        _builtTowers.Add(tower);

        tower.Enable();

        var mapPos = MapUtils.WorldToMap(snapped, Grid);

        foreach (var cell in CellSelector.GetOccupiedCells(mapPos, tower.Shape))
        {
            MapManager.SetTowerInCell(cell, tower);
        }

        _towerPreviewFactory.Return(_towerPreview);
    }

    public void TryDestroyTower(Tower tower)
    {
        if (tower == null) return;

        var snapped = MapUtils.SnapToGrid(tower.transform.position, Grid);
        var mapPos = MapUtils.WorldToMap(snapped, Grid);
        foreach (var cell in CellSelector.GetOccupiedCells(mapPos, tower.Shape))
        {
            MapManager.DestroyTowerInCell(cell);
        }
        _towerFactory.Return(tower);
    }

    private void CancelPlacement()
    {
        _towerPreviewFactory.Return(_towerPreview);
    }

    private bool IsValidPlacement(Vector3 worldPos)
    {
        Vector2Int mapPos = MapUtils.WorldToMap(worldPos, Grid);

        foreach (var cell in CellSelector.GetOccupiedCells(mapPos, _towerPreview.Shape))
        {
            if (!MapManager.IsInside(cell))
                return false;
            if (MapManager.IsCellBusy(cell))
                return false;
        }

        return true;
    }
}
