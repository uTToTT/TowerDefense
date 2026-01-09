using UnityEngine;

public class MapComposer : MonoBehaviour
{
    [SerializeField] private CellFactoryRegistry _factories;
    [SerializeField] private Transform _cellContainer;

    private void Awake()
    {
        _factories.Init();
    }

    //public void Build(MapData map)
    //{

    //    for (int y = 0; y < map.height; y++)
    //    {
    //        for (int x = 0; x < map.width; x++)
    //        {
    //            var type = map.Get(x, y);

    //            if (type == CellType.Empty)  continue; 

    //            var cell = _factories.Create(type);
    //            cell.transform.SetParent(_cellContainer);
    //            cell.transform.localPosition = MapUtils.GridToWorld(x, y, map);
    //        }
    //    }
    //}

    public void Build(MapData map, Grid grid)
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
            }
        }
    }
}
