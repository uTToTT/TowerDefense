using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "TowerFactoryRegistry",
    menuName = "TD/Tower/Tower Factory Registry")]
public class TowerFactoryRegistry : ScriptableObject
{
    [SerializeField] private TowerFactory[] _factories;

    private Dictionary<TowerType, TowerFactory> _map;

    public void Init()
    {
        _map = new Dictionary<TowerType, TowerFactory>();
        foreach (var factory in _factories)
        {
            factory.Init();
            _map.Add(factory.Type, factory);
        }
    }

    public Tower Create(TowerType type)
    {
        return _map[type].Create();
    }

    public void Return(Tower tower)
    {
        _map[tower.TowerType].Return(tower);
    }

    public void ReturnAll(Tower tower)
    {
        _map[tower.TowerType].ReturnAll();
    }

    public void Dispose(Tower tower)
    {
        _map[tower.TowerType].Dispose();
    }
}
