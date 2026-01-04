using UnityEngine;

public class MapComposer : MonoBehaviour
{
    [SerializeField] private MapData _map;
    [SerializeField] private float _cellSize = 2;
    [SerializeField] private CellFactoryRegistry _factories;
    [SerializeField] private Transform _cellContainer;

    private void Start()
    {
        _factories.Init();
        Build();
    }

    public void Build()
    {
        for (int y = 0; y < _map.height; y++)
            for (int x = 0; x < _map.width; x++)
            {
                var cell = _factories.Create(_map.Get(x, y));
                cell.transform.SetParent(_cellContainer);
                cell.transform.localPosition = GridToWorld(x , y);
            }
    }

    private Vector3 GridToWorld(int x, int y)
    {
        float xOffset = (_map.width - 1) * 0.5f;
        float yOffset = (_map.height - 1) * 0.5f;

        return new Vector3(
            (x - xOffset) * _cellSize,
            (y - yOffset) * _cellSize,
            0f
        );
    }

}
