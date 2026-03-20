using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "MapObjectPreviewFactoryRegistry",
    menuName = "TD/Map/Objects/Preview Factory Registry")]
public class MapObjectPreviewFactoryRegistry : ScriptableObject
{
    [SerializeField] private MapObjectPreviewFactory[] _factories;

    private Dictionary<MapObjectType, MapObjectPreviewFactory> _map;

    public void Init()
    {
        _map = new Dictionary<MapObjectType, MapObjectPreviewFactory>();
        foreach (var factory in _factories)
        {
            factory.Init();
            _map.Add(factory.Type, factory);
        }
    }

    public MapObject Create(MapObjectType type)
    {
        return _map[type].Create();
    }

    public void Return(MapObject tower)
    {
        _map[tower.Type].Return(tower);
    }

    public void ReturnAll(MapObject tower)
    {
        _map[tower.Type].ReturnAll();
    }

    public void Dispose(MapObject tower)
    {
        _map[tower.Type].Dispose();
    }
}
