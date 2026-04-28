using System;
using System.Collections.Generic;
using TToTT.TowerDefense.Enemies;

public sealed class EnemyTracker 
{
    public event Action<Enemy> EnemySpawned;
    public event Action<Enemy> EnemyDied;
    public event Action AllEnemiesDied;

    private readonly HashSet<Enemy> _alive = new();

    public int AliveCount => _alive.Count;

    public void Register(Enemy enemy)
    {
        if (_alive.Add(enemy))
        {
            EnemySpawned?.Invoke(enemy);
        }
    }

    public void Unregister(Enemy enemy)
    {
        if (_alive.Remove(enemy))
        {
            EnemyDied?.Invoke(enemy);

            if (_alive.Count == 0)
                AllEnemiesDied?.Invoke();
        }
    }

    public bool IsAlive(Enemy enemy)
    {
        return _alive.Contains(enemy);
    }

    public IReadOnlyCollection<Enemy> AliveEnemies => _alive;
}
