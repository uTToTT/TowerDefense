using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : Loader<EnemySpawner>
{
    [Header("Test")]
    [Space]
    [SerializeField] private bool _DISABLESPAWN;
    [Space]
    [Header("Time")]
    [SerializeField] private float _delayBeforeWave;
    [Space]
    [Header("Spawnpoints")]
    [SerializeField] private Waypoint[] _LRWaypoints;
    [SerializeField] private Waypoint _centralWaypoint;
    [Space]
    [Header("Move direction")]
    [SerializeField] private Direction _direction;
    [Space]
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _waveText;
    [Header("Waves")]
    [SerializeField] private WavesInfo _wavesInfo;
    [Space]
    [Header("Enemies")]
    [SerializeField] private Enemy _classicEnemy;
    [SerializeField] private Enemy _fastEnemy;
    [SerializeField] private Enemy _armorEnemy;
    [SerializeField] private Enemy _heavyEnemy;
    [SerializeField] private Enemy _kingEnemy;

    private List<Enemy> _enemies = new List<Enemy>();
    private Wave[] _waves;

    private Enemy _tmpEnemy;
    private Enemy _tmpEnemyPrefab;
    private bool _spawningNow;
    private bool _firstTowerWasBuilt;
    private bool _isNeedToSpawnInCentral;
    private int _waveCount;

    public List<Enemy> Enemies => _enemies;

    private void OnValidate()
    {
        _LRWaypoints[0].SetDirection(_direction);
        _LRWaypoints[1].SetDirection(_direction);
        _centralWaypoint.SetDirection(_direction);
    }

    private void Start()
    {
        _tmpEnemyPrefab = _classicEnemy;
        _waves = _wavesInfo.Waves;

        SetTextWave(0);
    }

    private void FixedUpdate()
    {
        if (_DISABLESPAWN)
        {
            Debug.Log("DEBUG - DISABLE SPAWN ENEMY");
        }
        else
        {
            if (_firstTowerWasBuilt && !_spawningNow && _enemies.Count == 0)
            {
                if (_waveCount < _waves.Length)
                {
                    StartCoroutine(Spawn());
                }
                else
                {
                    EventBus.GameWin?.Invoke();
                }
            }
        }
    }

    IEnumerator Spawn()
    {
        _spawningNow = true;

        SetTextWave(_waveCount + 1);

        EventBus.OnWaveStart?.Invoke(_waveCount);

        if (_waves[_waveCount].TypeOfEnemy == TypeEnemy.Fast)
        {
            _tmpEnemyPrefab = _fastEnemy;
            _isNeedToSpawnInCentral = false;
        }
        else if (_waves[_waveCount].TypeOfEnemy == TypeEnemy.Classic)
        {
            _tmpEnemyPrefab = _classicEnemy;
            _isNeedToSpawnInCentral = false;
        }
        else if (_waves[_waveCount].TypeOfEnemy == TypeEnemy.Armor)
        {
            _tmpEnemyPrefab = _armorEnemy;
            _isNeedToSpawnInCentral = false;
        }
        else if (_waves[_waveCount].TypeOfEnemy == TypeEnemy.Heavy)
        {
            _tmpEnemyPrefab = _heavyEnemy;
            _isNeedToSpawnInCentral = true;
        }
        else
        {
            _tmpEnemyPrefab = _kingEnemy;
            _isNeedToSpawnInCentral = true;
        }

        Enemy enemy = Instantiate(_tmpEnemyPrefab);

        enemy.SetMoveDirection(_direction);
        enemy.HPMultiply(_waves[_waveCount].HpMultiplier);
        enemy.MoneyDropMultiply(_waves[_waveCount].MoneyDropMultiplier);

        EventBus.onShowEnemyInfo?.Invoke(enemy);
        Destroy(enemy.gameObject);

        yield return new WaitForSeconds(_delayBeforeWave);

        for (int i = 0; i < _waves[_waveCount].EnemyCount; i++)
        {
            if (_isNeedToSpawnInCentral)
            {
                _tmpEnemy = Instantiate(_tmpEnemyPrefab, _centralWaypoint.transform.position, Quaternion.identity);
            }
            else
            {
                _tmpEnemy = Instantiate(_tmpEnemyPrefab, _LRWaypoints[i % 2].transform.position, Quaternion.identity);
            }

            _tmpEnemy.SetMoveDirection(_direction);
            _tmpEnemy.HPMultiply(_waves[_waveCount].HpMultiplier);
            _tmpEnemy.MoneyDropMultiply(_waves[_waveCount].MoneyDropMultiplier);

            RegisterEnemy(_tmpEnemy);

            yield return new WaitForSeconds(_waves[_waveCount].TimeBtwSpawn);
        }

        _waveCount++;

        _spawningNow = false;
    }

    private void SetTextWave(int waveCount)
    {
        _waveText.text = waveCount.ToString() + "\\" + _waves.Length.ToString();
    }

    private void StartSpawn()
    {
        StartCoroutine(Spawn());
        _firstTowerWasBuilt = true;
    }

    public void RegisterEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyEnemies()
    {
        foreach (Enemy item in _enemies)
        {
            Destroy(item.gameObject);
        }

        _enemies.Clear();
    }

    private void KillEnemies()
    {
        List<Enemy> enemiesCopy = new List<Enemy>(_enemies);

        foreach (Enemy item in enemiesCopy)
        {
            item.Death();
        }

        _enemies.Clear();
    }

    private void OnEnable()
    {
        EventBus.onAid += KillEnemies;
        EventBus.FirstTowerWasBuilt += StartSpawn;
    }

    private void OnDisable()
    {
        EventBus.onAid -= KillEnemies;
        EventBus.FirstTowerWasBuilt -= StartSpawn;
    }
}
