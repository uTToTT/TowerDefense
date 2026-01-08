using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyFactoryRegistry", menuName = "TD/Enemy/Enemy Factory Registry")]
public class EnemyFactoryRegistry : ScriptableObject
{
    [SerializeField] private EnemyFactory[] _factories;

    private Dictionary<EnemyType, EnemyFactory> _map;

    public void Init()
    {
        _map = new Dictionary<EnemyType, EnemyFactory>();
        foreach (var factory in _factories)
        {
            factory.Init();
            _map.Add(factory.EnemyType, factory);
        }
    }

    public Enemy Create(EnemyType type)
    {
        return _map[type].Create();
    }

    public void Return(Enemy enemy)
    {
        _map[enemy.EnemyType].Return(enemy);
    }

    public void ReturnAll(Enemy enemy)
    {
        _map[enemy.EnemyType].ReturnAll();
    }

    public void Dispose(Enemy enemy)
    {
        _map[enemy.EnemyType].Dispose();
    }
}
