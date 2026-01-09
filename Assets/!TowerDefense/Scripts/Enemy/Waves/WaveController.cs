using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveController : Loader<WaveController>
{
    [HorizontalLine]
    [SerializeField] private bool _enableSpawning = true;

    [HorizontalLine]
    [SerializeField, Range(0, 10)] private float _delayBeforeWave = 3;

    [HorizontalLine]
    [SerializeField] private MapData _mapData;
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private WavesData _wavesInfo;

    [HorizontalLine]
    [SerializeField] private EnemyFactoryRegistry _factories;

    private Enemy _tmpEnemy;
    private bool _spawningNow;
    private bool _firstTowerWasBuilt;
    private float _waveDelayTimer;
    private int _currWaveIndex = -1;
    private List<GroupRuntime> _activeGroups;
    private bool _waveActive;
    private WaveSpawnerState _state;
    public IReadOnlyCollection<Enemy> Enemies => _enemyTracker.AliveEnemies;

    private Coroutine _coroutine;
    private EnemyTracker _enemyTracker;

    private void Start() => Init();
    private void Update() => Process();

    private void Init()
    {
        _factories.Init();
        _enemyTracker = new EnemyTracker();
        SetTextWave(0);
        PrepareNextWave();
    }

    private void Process()
    {
        float dt = Time.deltaTime;

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
            PrepareNextWave();
        }
    }

    private void PrepareNextWave()
    {
        _currWaveIndex++;

        if (_currWaveIndex >= _wavesInfo.Waves.Length)
        {
            _state = WaveSpawnerState.Completed;
            Debug.Log("All waves completed");
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

        enemy.HPMultiply(group.HpMultiplier);
        enemy.MoneyDropMultiply(group.MoneyDropMultiplier);
        
        RegisterEnemy(enemy);

        EventBus.onShowEnemyInfo?.Invoke(enemy);

        Debug.Log(
           $"Spawn {group.EnemyType} | Route: {group.Route} | Lane: {group.Lane}"
       );
    }

    private void SetTextWave(int waveCount)
    {
        _waveText.text = waveCount.ToString() + "\\" + _wavesInfo.Waves.Length.ToString();
    }

    public void RegisterEnemy(Enemy enemy) => _enemyTracker.Register(enemy);
    public void UnregisterEnemy(Enemy enemy) => _enemyTracker.Unregister(enemy);

    private void KillEnemies()
    {
        List<Enemy> enemiesCopy = new List<Enemy>(Enemies);

        foreach (Enemy item in enemiesCopy)
        {
            item.Death();
        }

        //Enemies.Clear();
    }

    private void OnEnable()
    {
        EventBus.onAid += KillEnemies;
    }

    private void OnDisable()
    {
        EventBus.onAid -= KillEnemies;
    }
}
