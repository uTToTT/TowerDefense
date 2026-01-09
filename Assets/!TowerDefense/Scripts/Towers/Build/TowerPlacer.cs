using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private TowerFactoryRegistry _factories;
    [SerializeField] private LayerMask _placementMask;
    [SerializeField] private TowerBuildButton[] _buildButtons;

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

        Vector3 snapped = SnapToGrid(worldPos);
        _previewTower.transform.position = snapped;
    }


    private void TryPlaceTower()
    {
        _isDragging = false;

        if (!IsValidPlacement(_previewTower.transform.position))
        {
            CancelPlacement();
            return;
        }

        var tower = _factories.Create(_draggingType);
        tower.transform.position = _previewTower.transform.position;
        Destroy(_previewTower);
    }

    private void CancelPlacement()
    {
        Destroy(_previewTower);
    }

    private Vector3 SnapToGrid(Vector3 worldPos)
    {
        Vector3Int cell = _grid.WorldToCell(worldPos);
        return _grid.GetCellCenterWorld(cell);
    }

    private bool IsValidPlacement(Vector3 position)
    {
        return true;
    }
}
