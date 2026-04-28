using System.Collections.Generic;

public class EnemyManager 
{
    private readonly WaveController _waveController;

    private readonly List<Enemy> _enemies = new();
    private readonly List<Enemy> _toAdd = new();
    private readonly List<Enemy> _toRemove = new();

    public int AliveCount => _enemies.Count;

    #region Init

    public EnemyManager(WaveController waveController)
    {
        _waveController = waveController;
    }

    #endregion

    #region Game loop

    public void Restart()
    {
        _waveController.Restart();
        _enemies.Clear();
        _toAdd.Clear();
        _toRemove.Clear();
    }

    public void Tick(float dt)
    {
        UpdateColleciton();

        foreach (var enemy in _enemies)
        {
            if (enemy != null && enemy.IsAlive)
                enemy.Tick(dt);
        }

        _waveController.Tick(dt);
    }

    #endregion

    public void Register(Enemy enemy) => _toAdd.Add(enemy);
    public void Unregister(Enemy enemy) => _toRemove.Add(enemy);

    private void UpdateColleciton()
    {
        if (_toRemove.Count > 0)
        {
            foreach (var item in _toRemove)
                _enemies.Remove(item);

            _toRemove.Clear();
        }

        if (_toAdd.Count > 0)
        {
            foreach (var item in _toAdd)
                _enemies.Add(item);

            _toAdd.Clear();
        }
    }
}
