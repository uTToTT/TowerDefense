using UnityEngine;

public class CellSelector : MonoBehaviour
{
    public void Init()
    {
    }

    public void OnTapCanceled()
    {
        MapManager.Instance.ClearSellection();
        MapManager.Instance.HideMapObjectPorts();
        UnselectTower();
    }

    public void OnTapPerformed()
    {
        var cellData = MapManager.Instance.Raycast();

        MapManager.Instance.ClearSellection();

        if (cellData != null &&
            cellData.MapObject != null)
        {
            var mapObject = cellData.MapObject;

            MapManager.Instance.DrawBorderMapObject(mapObject);

            if (mapObject is Tower tower)
            {
                SelectTower(tower);
                MapManager.Instance.ShowMapObjectPorts(tower);
            }
        }
    }

    private void SelectTower(Tower tower)
    {
        GameManager.Instance.TowerManager.SelectTower(tower);
    }

    private void UnselectTower()
    {
        GameManager.Instance.TowerManager.UnselectTower();
    }
}
