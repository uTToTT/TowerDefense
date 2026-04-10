using NaughtyAttributes;
using System.Collections.Generic;
using TToTT.TowerDefense.Map;
using UnityEngine;

public class WaveController 
{
    private readonly IWaveView _waveView;

    [SerializeField] private WavesData _wavesInfo;

    [HorizontalLine]
    [SerializeField] private EnemyFactoryRegistry _factories;

    private float _delayBeforeWave;
    private float _waveDelayTimer;
    private int _currWaveIndex = -1;
    private int _spawnedEnemyCount;
    private List<GroupRuntime> _activeGroups;
    private WaveSpawnerState _state;

    public bool IsPlayerWaveStarted { get; set; }
    public bool IsWaveEnded { get; set; }
    public bool IsAllWavesCompleted { get; set; }

    public int CurrWave => _currWaveIndex + 1;
    public int LastWave => _wavesInfo.Waves.Length;

    public WaveController()
    {
        _factories.Init();
        _delayBeforeWave = 3;
    }

    public void Restart()
    {
        _factories.ReturnAll();
        _currWaveIndex = -1;
        _spawnedEnemyCount = 0;
        _waveDelayTimer = 0;

        IsPlayerWaveStarted = false;
        IsWaveEnded= false;
        IsAllWavesCompleted = false;
        _state = WaveSpawnerState.WaitingForNextWave;

        PrepareNextWave();
    }

    public void Tick(float dt)
    {
        if (!IsPlayerWaveStarted ||
            IsWaveEnded ||
            IsAllWavesCompleted)
        {
            return;
        }

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

        UpdateWaveText();

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

        PathLane lane;

        if (group.Lane == PathLane.LeftRight)
            lane = _spawnedEnemyCount % 2 == 0 ? PathLane.Left : PathLane.Right;
        else
            lane = group.Lane;

        enemy.SetLane(lane);
        enemy.BuildRoute(MapManager.Instance.GetRoutePoints(group.Route));

        enemy.OnDeath += OnEnemyDeath;

        EnemyManager.Instance.Register(enemy);

        _spawnedEnemyCount++;

        // Debug.Log(
        //    $"Spawn {group.EnemyType} | Route: {group.Route} | Lane: {group.Lane}"
        //);
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        enemy.OnDeath -= OnEnemyDeath;
        _factories.Return(enemy);
        EnemyManager.Instance.Unregister(enemy);
    }

    private void UpdateWaveText() => _waveText.text = "Wave " + CurrWave + "\\" + LastWave;

    public void PlayerStartWave() => IsPlayerWaveStarted = true;
    public void StopWave() => IsPlayerWaveStarted = false;

    private void AllWavesCompleted() => IsAllWavesCompleted = true;
    private void WaveEnded() => IsWaveEnded = true;
}
