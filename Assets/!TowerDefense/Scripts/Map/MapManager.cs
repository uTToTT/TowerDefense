using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapComposer _mapComposer;
    [SerializeField] private MapData _mapData;

    private void Start()
    {
        _mapComposer.Build(_mapData);
    }
}
