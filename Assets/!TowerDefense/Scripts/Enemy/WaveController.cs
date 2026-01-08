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
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private WavesInfo _wavesInfo;

    [HorizontalLine]
    [SerializeField] private EnemyFactoryRegistry _factories;

    private Enemy _tmpEnemy;
    private bool _spawningNow;
    private bool _firstTowerWasBuilt;
    private int _waveCount;

    public IReadOnlyCollection<Enemy> Enemies => _enemyTracker.AliveEnemies;

    private Coroutine _coroutine;
    private EnemyTracker _enemyTracker;

    private void Start() => Init();

    private void FixedUpdate()
    {
        if (!_enableSpawning) return;

        if (_firstTowerWasBuilt && !_spawningNow && Enemies.Count == 0)
        {
            if (_waveCount < _wavesInfo.Waves.Length)
            {
                StartCoroutine(Spawn());
            }
            else
            {
                EventBus.GameWin?.Invoke();
            }
        }
    }

    private void Init()
    {
        _enemyTracker = new EnemyTracker();
        SetTextWave(0);
    }

    private IEnumerator Spawn()
    {
        _spawningNow = true;

        SetTextWave(_waveCount + 1);

        EventBus.OnWaveStart?.Invoke(_waveCount);

        Enemy enemy = _factories.Create(_wavesInfo.Waves[_waveCount].Groups[0].EnemyType);

        enemy.HPMultiply(_wavesInfo.Waves[_waveCount].Groups[0].HpMultiplier);
        enemy.MoneyDropMultiply(_wavesInfo.Waves[_waveCount].Groups[0].MoneyDropMultiplier);

        EventBus.onShowEnemyInfo?.Invoke(enemy);
        Destroy(enemy.gameObject);

        yield return new WaitForSeconds(_delayBeforeWave);

        for (int i = 0; i < _wavesInfo.Waves[_waveCount].Groups[0].EnemyCount; i++)
        {
            _tmpEnemy.HPMultiply(_wavesInfo.Waves[_waveCount].Groups[0].HpMultiplier);
            _tmpEnemy.MoneyDropMultiply(_wavesInfo.Waves[_waveCount].Groups[0].MoneyDropMultiplier);

            RegisterEnemy(_tmpEnemy);

            yield return new WaitForSeconds(_wavesInfo.Waves[_waveCount].Groups[0].TimeBtwSpawn);
        }

        _waveCount++;

        _spawningNow = false;
    }

    private void SetTextWave(int waveCount)
    {
        _waveText.text = waveCount.ToString() + "\\" + _wavesInfo.Waves.Length.ToString();
    }

    private void StartSpawn()
    {
        if (_coroutine == null)
            _coroutine = StartCoroutine(Spawn());

        _firstTowerWasBuilt = true;
    }

    private void StopSpawn()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
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
        EventBus.FirstTowerWasBuilt += StartSpawn;
    }

    private void OnDisable()
    {
        EventBus.onAid -= KillEnemies;
        EventBus.FirstTowerWasBuilt -= StartSpawn;
    }
}
