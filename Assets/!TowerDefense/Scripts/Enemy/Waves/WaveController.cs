using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveController : Loader<WaveController>
{
    public event Action<float> OnMoneyDropped;

    [HorizontalLine]
    [SerializeField, Range(0, 10)] private float _delayBeforeWave = 3;

    [HorizontalLine]
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private WavesData _wavesInfo;
    [SerializeField] private MoveManager _moveManager;
    [HorizontalLine]
    [SerializeField] private EnemyFactoryRegistry _factories;

    private float _waveDelayTimer;
    private int _currWaveIndex = -1;
    private List<GroupRuntime> _activeGroups;
    private WaveSpawnerState _state;
    public IReadOnlyCollection<Enemy> Enemies => _enemyTracker.AliveEnemies;

    private EnemyTracker _enemyTracker;

    private bool _isPlayerWaveStarted;

    public void Init()
    {
        _factories.Init();
        _enemyTracker = new EnemyTracker();
        SetTextWave(0);
        PrepareNextWave();
    }

    public void Tick(float dt)
    {
        if (!_isPlayerWaveStarted) return;

        switch (_state)
        {
            case WaveSpawnerState.WaitingForNextWave:
                UpdateWaveDelay(dt);
                break;

            case WaveSpawnerState.Spawning:
                UpdateSpawning(dt);
                break;
        }
    }

    private void UpdateWaveDelay(float dt)
    {
        _waveDelayTimer -= dt;

        if (_waveDelayTimer <= 0f)
        {
            StartWave();
        }
    }

    private void UpdateSpawning(float dt)
    {
        bool allGroupsCompleted = true;

        foreach (var group in _activeGroups)
        {
            if (!group.IsCompleted)
            {
                allGroupsCompleted = false;

                if (group.CanSpawn(dt))
                {
                    SpawnEnemy(group.Data);
                }
            }
        }

        if (allGroupsCompleted)
        {
            WaveEnded();
            PrepareNextWave();
        }
    }

    private void PrepareNextWave()
    {
        _currWaveIndex++;

        if (_currWaveIndex >= _wavesInfo.Waves.Length)
        {
            _state = WaveSpawnerState.Completed;
            AllWavesCompleted();
            return;
        }

        Wave wave = _wavesInfo.Waves[_currWaveIndex];

        _waveDelayTimer = _delayBeforeWave;
        _state = WaveSpawnerState.WaitingForNextWave;

        Debug.Log($"Wave {_currWaveIndex + 1} will start in {_delayBeforeWave} sec");
    }


    private void StartWave()
    {
        Wave wave = _wavesInfo.Waves[_currWaveIndex];

        _activeGroups = new List<GroupRuntime>(wave.Groups.Length);

        foreach (var group in wave.Groups)
        {
            _activeGroups.Add(new GroupRuntime(group));
        }

        _state = WaveSpawnerState.Spawning;

        Debug.Log($"Wave {_currWaveIndex + 1} started");
    }

    private void SpawnEnemy(Group group)
    {
        Enemy enemy = _factories.Create(group.EnemyType);
        enemy.transform.position =
            MapUtils.GridToWorld(
                MapManager.Instance.GetRoute(group.Route).entrance,
                MapManager.Instance.Grid);
        enemy.HPMultiply(group.HpMultiplier);
        enemy.MoneyDropMultiply(group.MoneyDropMultiplier);
        enemy.BuildRoute(MapManager.Instance.GetRoutePoints(group.Route));
        enemy.SetLane(group.Lane);
        enemy.OnDeath += OnEnemyDeath;

        _moveManager.Register(enemy);
        RegisterEnemy(enemy);

        EventBus.onShowEnemyInfo?.Invoke(enemy);

        // Debug.Log(
        //    $"Spawn {group.EnemyType} | Route: {group.Route} | Lane: {group.Lane}"
        //);
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        enemy.OnDeath -= OnEnemyDeath;
        _factories.Return(enemy);
    }


    private void SetTextWave(int waveCount)
    {
        _waveText.text = waveCount.ToString() + "\\" + _wavesInfo.Waves.Length.ToString();
    }

    public void RegisterEnemy(Enemy enemy) => _enemyTracker.Register(enemy);
    public void UnregisterEnemy(Enemy enemy) => _enemyTracker.Unregister(enemy);

    public void PlayerStartWave() => _isPlayerWaveStarted = true;
    public void StopWave() => _isPlayerWaveStarted = false;

    private void AllWavesCompleted() => GameManager.Instance.AllWavesEnded();
    private void WaveEnded()
    {
        GameManager.Instance.WaveEnded();
    }
}
