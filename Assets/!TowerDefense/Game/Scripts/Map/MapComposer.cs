using UnityEngine;

public class MapComposer : MonoBehaviour
{
    [SerializeField] private CellFactoryRegistry _factories;
    [SerializeField] private Transform _cellContainer;

    private void Awake()
    {
        _factories.Init();
    }

    public void Build(MapData map, CellData[,] cellDatas, Grid grid)
    {
        for (int y = 0; y < map.height; y++)
        {
            for (int x = 0; x < map.width; x++)
            {
                var type = map.Get(x, y);
                if (type == CellType.Empty)
                    continue;

                var cell = _factories.Create(type);
                cell.transform.SetParent(_cellContainer);

                Vector3Int cellPos = new Vector3Int(x, y, 0);
                cell.transform.position = grid.GetCellCenterWorld(cellPos);

                cellDatas[x, y] = new CellData();
                cellDatas[x, y].CellType = cell.CellType;
                if (cell.CellType == CellType.Path ||
                    cell.CellType == CellType.Entrance ||
                    cell.CellType == CellType.Exit ||
                    cell.CellType == CellType.Blocked)
                {
                    cellDatas[x, y].IsBusy = true;
                }
            }
        }
    }
}
