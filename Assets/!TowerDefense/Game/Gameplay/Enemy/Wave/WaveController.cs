using System;
using System.Collections.Generic;
using TToTT.TowerDefense.UI.Label;

namespace TToTT.TowerDefense.Enemies.Wave
{
    public class WaveController
    {
        public event Action OnAllWavesCompleted;
        public event Action OnWaveCleared;

        private readonly WaveStateMachine _state;
        private readonly EnemySpawner _spawner;
        private readonly ILabelView _waveText;
        private readonly IAnalyticsService _analyticsService;

        private float _delayBeforeWave;
        private float _waveDelayTimer;
        private int _currWaveIndex = -1;
        private int _aliveCount = 0;
        private int _killedCount = 0;
        private bool _spawningFinished;

        private List<GroupRuntime> _activeGroups;
        private WavesData _waves;

        public int CurrWave => _currWaveIndex + 1;
        public int LastWave => _waves.Waves.Length;

        public WaveController(
            WaveStateMachine state,
            EnemySpawner spawner,
            LabelRegistry labels,
            IAnalyticsService analyticsService)
        {
            _delayBeforeWave = 3;
            _state = state;
            _spawner = spawner;
            _waveText = labels.Get(LabelId.Wave);
            _analyticsService = analyticsService;

            _spawner.OnSpawned += OnEnemySpawned;
            _spawner.OnDeath += OnEnemyDied;
        }

        public void InitData(WavesData waves) => _waves = waves;

        public void Restart()
        {
            _spawner.Restart();
            _currWaveIndex = -1;
            _waveDelayTimer = 0;
            _aliveCount = 0;
            _killedCount = 0;
            _spawningFinished = false;
            _state.SetState(WaveState.Pause);
        }

        public void Tick(float dt)
        {
            if (_state.State == WaveState.Pause ||
                _state.State == WaveState.Completed) return;

            switch (_state.State)
            {
                case WaveState.Start:
                    UpdateWaveDelay(dt);
                    break;
                case WaveState.Spawning:
                    UpdateSpawning(dt);
                    break;
            }
        }

        public void HandleGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.GameplayLoading:
                    PrepareNextWave();
                    break;
                case GameState.Wave:
                    _state.SetState(WaveState.Start);
                    break;
            }
        }

        private void OnEnemySpawned(Enemy enemy)
        {
            _aliveCount++;
        }

        private void OnEnemyDied(Enemy enemy)
        {
            _aliveCount--;
            _killedCount++;
            CheckWaveCleared();
        }

        private void CheckWaveCleared()
        {
            if (!_spawningFinished) return;
            if (_aliveCount > 0) return;

            OnWaveCleared?.Invoke();
            _analyticsService.TrackWaveCompleted(_currWaveIndex + 1, _killedCount);
            PrepareNextWave();
        }

        private void UpdateWaveDelay(float dt)
        {
            _waveDelayTimer -= dt;
            if (_waveDelayTimer <= 0f)
                StartWave();
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
                        _spawner.Spawn(group.Data);
                }
            }

            if (allGroupsCompleted)
            {
                _spawningFinished = true;
                _state.SetState(WaveState.WaitingForClear);
                CheckWaveCleared();
            }
        }

        private void PrepareNextWave()
        {
            _currWaveIndex++;

            if (_currWaveIndex >= _waves.Waves.Length)
            {
                _state.SetState(WaveState.Completed);
                OnAllWavesCompleted?.Invoke();
                return;
            }

            _aliveCount = 0;
            _killedCount = 0;
            _spawningFinished = false;
            _waveDelayTimer = _delayBeforeWave;
            _waveText.SetText($"Wave {_currWaveIndex + 1}/{LastWave}");
        }

        private void StartWave()
        {
            Wave wave = _waves.Waves[_currWaveIndex];
            _activeGroups = new List<GroupRuntime>(wave.Groups.Length);

            foreach (var group in wave.Groups)
                _activeGroups.Add(new GroupRuntime(group));

            _state.SetState(WaveState.Spawning);
        }
    }
}