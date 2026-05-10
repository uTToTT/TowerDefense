using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellFactoryRegistry", menuName = "TD/Map/Cell Factory Registry")]
public class CellFactoryRegistry : ScriptableObject
{
    [SerializeField] private CellFactory[] _factories;

    private Dictionary<CellType, CellFactory> _map;
    private bool _initialized;

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
        Cell cell = null;

        try
        {
            cell = _map[type].Create();
        }
        catch 
        {

            throw new System.Exception($"Cell with type [{type}] can't be created.");
        }


        return cell;
    }

    public void Return(Cell cell)
    {
        _map[cell.CellType].Return(cell);
    }

    public void ReturnAll()
    {
        foreach (var factory in _factories)
        {
            factory.ReturnAll();
        }
    }

    public void Dispose(Cell cell)
    {
        _map[cell.CellType].Dispose();
    }
}
