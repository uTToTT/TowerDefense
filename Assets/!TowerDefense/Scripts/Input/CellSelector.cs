using System.Collections.Generic;
using UnityEngine;

public class CellSelector : MonoBehaviour
{
    [SerializeField] private CellSelectionFactory _selectionFactory;
    [SerializeField] private SelectionMenu _selectionMenu;

    private List<CellSelection> _selections = new();

    private Tower _selectedTower;
    public bool IsTowerSelected => _selectedTower != null;

    public void Init()
    {
        _selectionFactory.Init();
        _selectionMenu.OnUpgrade += UpgradeTower;
    }

    private void UpgradeTower(UpgradeNodeConfig upgrade)
    {
        if (IsTowerSelected) _selectedTower.UpgradeController.Purchase(upgrade);
    }

    public void OnTap()
    {
        if (GameManager.Instance.PlayerInputController.IsPointerOverUI())
            return;

        var worldPos = GameManager.Instance.PlayerInputController.GetPointerPosition();
        var mapPos = MapUtils.WorldToMap(worldPos, MapManager.Instance.Grid);

        ClearSellection();

        var cellData = MapManager.Instance.GetCellData(mapPos);

        if (cellData != null &&
            cellData.MapObject != null)
        {
            var mapObject = cellData.MapObject;
            var occupiedCells = GetOccupiedCells(mapObject.Anchor, mapObject.Shape);

            for (int i = 0; i < occupiedCells.Count; i++)
            {
                var seleciton = _selectionFactory.Create();
                seleciton.transform.position =
                    MapUtils.MapToWorld(
                        occupiedCells[i],
                        MapManager.Instance.Grid);
                _selections.Add(seleciton);
            }

            if (mapObject is Tower tower)
            {
                SelectTower(tower);
            }
        }
        else
        {
            var seleciton = _selectionFactory.Create();
            seleciton.transform.position = MapUtils.SnapToGrid(worldPos, MapManager.Instance.Grid);
            _selections.Add(seleciton);
        }

        _selectionMenu.Enable();
        _selectionMenu.transform.position = MapUtils.SnapToGrid(worldPos, MapManager.Instance.Grid);
    }

    private void SelectTower(Tower tower)
    {
        _selectedTower = tower;
        _selectionMenu.CreateUpgradeButtons(tower.UpgradeController.CurrentUpgrade.Next);
    }

    private void ClearSellection()
    {
        if (_selections.Count < 0) return;

        for (int i = _selections.Count - 1; i >= 0; i--)
        {
            _selectionFactory.Return(_selections[i]);
        }

        _selections.Clear();
        _selectionMenu.Disable();

        _selectedTower = null;
    }

    public static List<Vector2Int> GetOccupiedCells(
    Vector2Int anchor,
    MapObjectShape shape)
    {
        var result = new List<Vector2Int>();

        foreach (var offset in shape.OccupiedCells)
        {
            result.Add(new Vector2Int(
                anchor.x + offset.X,
                anchor.y + offset.Y
            ));
        }

        return result;
    }
}
