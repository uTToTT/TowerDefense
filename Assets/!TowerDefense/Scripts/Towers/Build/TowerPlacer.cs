using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private TowerFactoryRegistry _factories;
    [SerializeField] private LayerMask _placementMask;
    [SerializeField] private TowerBuildButton[] _buildButtons;
    [SerializeField] private MapManager _mapManager;

    private GameObject _previewTower;
    private TowerType _draggingType;
    private bool _isDragging;

    private void Start()
    {
        foreach (var button in _buildButtons)
        {
            button.TowerPlacer = this;
        }
        _factories.Init();
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
        _previewTower = _factories.Create(towerType).gameObject;
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

        _previewTower.transform.position =
            MapUtils.SnapToGrid(worldPos, _grid);
    }

    private void TryPlaceTower()
    {
        _isDragging = false;

        var snapped = MapUtils.SnapToGrid(_previewTower.transform.position, _grid);

        if (!IsValidPlacement(snapped))
        {
            CancelPlacement();
            return;
        }

        var tower = _factories.Create(_draggingType);
        var mapPos = MapUtils.WorldToMap(snapped, _grid);

        _mapManager.SetBusyState(mapPos, true);
        tower.transform.position = _previewTower.transform.position;

        Destroy(_previewTower);
    }

    private void CancelPlacement()
    {
        Destroy(_previewTower);
    }

    private bool IsValidPlacement(Vector3 worldPos)
    {
        Vector2Int mapPos = MapUtils.WorldToMap(worldPos, _grid);

        return _mapManager.IsInside(mapPos) &&
            !_mapManager.IsCellBusy(mapPos);
    }
}
