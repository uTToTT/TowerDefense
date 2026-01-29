using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float _delayBeforeWave = 3;
    [SerializeField] private WaveController _waveController;

    private readonly List<Enemy> _enemies = new();
    private readonly List<Enemy> _toAdd = new();
    private readonly List<Enemy> _toRemove = new();

    public int AliveCount => _enemies.Count;

    public static EnemyManager Instance { get; private set; }
    public WaveController WaveController => _waveController;

    public void Init()
    {
        Instance = this;
        _waveController.Init(_delayBeforeWave);
    }

    public void Tick(float dt)
    {
        UpdateColleciton();
        UpdateWaveState();

        foreach (var enemy in _enemies)
        {
            if (enemy != null && enemy.IsAlive)
                enemy.Tick(dt);
        }

        _waveController.Tick(dt);
    }

    public void Register(Enemy enemy) => _toAdd.Add(enemy);
    public void Unregister(Enemy enemy) => _toRemove.Add(enemy);

    private void UpdateWaveState()
    {
        if (AliveCount <= 0)
        {
            if (_waveController.IsAllWavesCompleted)
            {
                _waveController.IsAllWavesCompleted = false;
                GameManager.Instance.AllWavesEnded();
            }
            else if (_waveController.IsWaveEnded)
            {
                _waveController.IsWaveEnded = false;
                GameManager.Instance.WaveEnded();
            }
        }
    }

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
