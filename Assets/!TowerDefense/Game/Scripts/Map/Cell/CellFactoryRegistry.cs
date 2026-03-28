using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellFactoryRegistry",menuName = "TD/Map/Cell Factory Registry")]
public class CellFactoryRegistry : ScriptableObject
{
    [SerializeField] private CellFactory[] _factories;

    private Dictionary<CellType, CellFactory> _map;
    private bool _initialized;

    public void Init()
    {
        if (_initialized) return;

        _map = new Dictionary<CellType, CellFactory>();
        foreach (var factory in _factories)
        {
            factory.Init();
            _map.Add(factory.CellType, factory);
        }

        _initialized = true;
    }

    public Cell Create(CellType type)
    {
        Init();
        return _map[type].Create();
    }

    public void Return(Cell cell)
    {
        Init();
        _map[cell.CellType].Return(cell);
    }

    public void ReturnAll(Cell cell)
    {
        Init();
        _map[cell.CellType].ReturnAll();
    }

    public void Dispose(Cell cell)
    {
        Init();
        _map[cell.CellType].Dispose();
    }
}
