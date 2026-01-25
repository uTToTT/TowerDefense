using System.Collections.Generic;
using UnityEngine;

public class CellSelector : MonoBehaviour
{
    [SerializeField] private CellSelectionFactory _selectionFactory;

    private List<CellSelection> _selections = new();


    public void Init()
    {
        _selectionFactory.Init();
    }

    public void OnTap()
    {
        if (GameManager.Instance.PlayerInputController.IsPointerOverUI())
            return;

        var v2 = GameManager.Instance.PlayerInputController.GetPointerPosition();

        ClearSellection();

        var mapPos = MapUtils.WorldToMap(v2, MapManager.Instance.Grid);
        var cellData = MapManager.Instance.GetCellData(mapPos);

        if (cellData != null &&
            cellData.MapObject != null)
        {
            var mapObject = cellData.MapObject;
            var occupiedCells = GetOccupiedCells(mapObject.Anchor, mapObject.Shape);

            for (int i = 0; i < occupiedCells.Count; i++)
            {
                var seleciton = _selectionFactory.Create();
                seleciton.transform.position = new Vector3(occupiedCells[i].x, occupiedCells[i].y);
                _selections.Add(seleciton);
            }
        }
    }

    private void ClearSellection()
    {
        if (_selections.Count < 0) return;

        for (int i = _selections.Count; i > 0; i--)
        {
            _selectionFactory.Return(_selections[i]);
        }

        _selections.Clear();
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
