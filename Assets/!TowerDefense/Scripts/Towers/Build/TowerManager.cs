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

    private Tower _selectedTower;
    private Vector2 _lastTowerValidPosition;

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
        if (_selectedTower != null)
        {
            MapUtils.SnapToGridUnderPointer(_selectedTower.transform);
            if (IsValidPlacement(_selectedTower.transform.position))
                _lastTowerValidPosition = _selectedTower.transform.position;
        }

        foreach (var tower in _builtTowers)
            tower.Tick(dt);

        if (!_isDragging)
            return;

        UpdatePreviewPosition();

        if (GameManager.Instance.PlayerInputController.IsPointerDown == false)
            TryPlaceTower();
    }

    public void SelectTower(Tower tower)
    {
        if (tower == null) return;
        if (GameManager.Instance.IsBattle) return;

        tower.Disable();
        _selectedTower = tower;
        _lastTowerValidPosition = _selectedTower.transform.position;

        foreach (var cell in MapManager.GetOccupiedCells(tower.MapPos, tower.Shape))
        {
            MapManager.RemoveTowerInCell(cell);
        }
    }

    public void UnselectTower()
    {
        if (_selectedTower == null) return;

        PlaceTower(_selectedTower, _lastTowerValidPosition);
        _selectedTower.Enable();

        _selectedTower = null;
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
        MapUtils.SnapToGridUnderPointer(_towerPreview.transform);

        if (IsValidPlacement(_towerPreview.transform.position))
            _lastTowerValidPosition = _towerPreview.transform.position;
    }

    /// <returns>Return snapped position</returns>
    private void PlaceTower(Tower tower, Vector2 pos)
    {
        tower.transform.position = pos;

        var mapPos = MapUtils.WorldToMap(_lastTowerValidPosition, Grid);
        tower.MapPos = mapPos; 

        foreach (var cell in MapManager.GetOccupiedCells(mapPos, tower.Shape))
        {
            MapManager.SetTowerInCell(cell, tower);
        }
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
        PlaceTower(tower, snapped);
        _builtTowers.Add(tower);

        tower.Enable();

        _towerPreviewFactory.Return(_towerPreview);
    }

    private void CancelPlacement()
    {
        _towerPreviewFactory.Return(_towerPreview);
    }

    private bool IsValidPlacement(Vector3 worldPos)
    {
        Vector2Int mapPos = MapUtils.WorldToMap(worldPos, Grid);

        foreach (var cell in MapManager.GetOccupiedCells(mapPos, _towerPreview.Shape))
        {
            if (!MapManager.IsInside(cell))
                return false;
            if (MapManager.IsCellBusy(cell))
                return false;
        }

        return true;
    }
}
