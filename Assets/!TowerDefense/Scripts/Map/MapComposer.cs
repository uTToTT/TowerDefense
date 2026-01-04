using System.Collections.Generic;
using UnityEngine;

public class MapComposer : MonoBehaviour
{
    [SerializeField] private CellFactoryRegistry _factories;
    [SerializeField] private Transform _cellContainer;

    private void Awake()
    {
        _factories.Init();
    }

    public void Build(MapData map)
    {

        for (int y = 0; y < map.height; y++)
        {
            for (int x = 0; x < map.width; x++)
            {
                var cell = _factories.Create(map.Get(x, y));
                cell.transform.SetParent(_cellContainer);
                cell.transform.localPosition = MapUtils.GridToWorld(x, y, map);
            }
        }
    }
}
