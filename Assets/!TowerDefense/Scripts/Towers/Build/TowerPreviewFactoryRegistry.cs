using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "TowerPreviewFactoryRegistry",
    menuName = "TD/Tower/Tower Preview Factory Registry")]
public class TowerPreviewFactoryRegistry : ScriptableObject
{
    [SerializeField] private TowerPreviewFactory[] _factories;

    private Dictionary<TowerType, TowerPreviewFactory> _map;

    public void Init()
    {
        _map = new Dictionary<TowerType, TowerPreviewFactory>();
        foreach (var factory in _factories)
        {
            factory.Init();
            _map.Add(factory.Type, factory);
        }
    }

    public TowerPreview Create(TowerType type)
    {
        return _map[type].Create();
    }

    public void Return(TowerPreview tower)
    {
        _map[tower.TowerType].Return(tower);
    }

    public void ReturnAll(TowerPreview tower)
    {
        _map[tower.TowerType].ReturnAll();
    }

    public void Dispose(TowerPreview tower)
    {
        _map[tower.TowerType].Dispose();
    }
}
