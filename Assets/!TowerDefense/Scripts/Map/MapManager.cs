using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private MapComposer _mapComposer;
    [SerializeField] private MapData _mapData;

    private void Start()
    {
        CenterGrid(_mapData);
        _mapComposer.Build(_mapData, _grid);
    }

    private void CenterGrid(MapData map)
    {
        Vector3 gridSize = new Vector3(
            map.width * _grid.cellSize.x,
            map.height * _grid.cellSize.y,
            0f
        );

        _grid.transform.position =
            -gridSize * 0.5f +
            new Vector3(
                _grid.cellSize.x * 0.5f,
                _grid.cellSize.y * 0.5f,
                0f
            );
    }

}
