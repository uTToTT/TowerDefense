using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "MapObjectFactoryRegistry",
    menuName = "TD/Map/Map Object Factory Registry")]
public class MapObjectFactoryRegistry : ScriptableObject
{
    [SerializeField] private MapObjectFactory[] _factories;

    private Dictionary<MapObjectType, MapObjectFactory> _map;

    public void Init()
    {
        _map = new Dictionary<MapObjectType, MapObjectFactory>();
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
