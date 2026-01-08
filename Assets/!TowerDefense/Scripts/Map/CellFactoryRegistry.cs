using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/Map/Cell Factory Registry")]
public class CellFactoryRegistry : ScriptableObject
{
    [SerializeField] private CellFactory[] _factories;

    private Dictionary<CellType, CellFactory> _map;

    public void Init()
    {
        _map = new Dictionary<CellType, CellFactory>();
        foreach (var factory in _factories)
        {
            factory.Init();
            _map.Add(factory.CellType, factory);
        }
    }

    public Cell Create(CellType type)
    {
        return _map[type].Create();
    }

    public void Return(Cell cell)
    {
        _map[cell.CellType].Return(cell);
    }

    public void ReturnAll(Cell cell)
    {
        _map[cell.CellType].ReturnAll();
    }

    public void Dispose(Cell cell)
    {
        _map[cell.CellType].Dispose();
    }
}
