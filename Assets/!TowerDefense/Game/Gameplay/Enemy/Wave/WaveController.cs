using System.Collections.Generic;

namespace TToTT.TowerDefense.Enemies
{
    public class WaveController
    {
        private readonly WaveStateMachine _state;
        private readonly EnemySpawner _spawner;

        private float _delayBeforeWave; // TODO: replace to config
        private float _waveDelayTimer;
        private int _currWaveIndex = -1;

        private List<GroupRuntime> _activeGroups;
        private WavesData _waves;

        public int CurrWave => _currWaveIndex + 1;
        public int LastWave => _waves.Waves.Length;

        public WaveController(
            WaveStateMachine state,
            EnemySpawner spawner)
        {
            _delayBeforeWave = 3;
            _state = state;
            _spawner = spawner;
        }

        public void InitData(WavesData waves)
        {
            _waves = waves;
        }

        public void Restart()
        {
            _spawner.Restart();

            _currWaveIndex = -1;
            _waveDelayTimer = 0;

            _state.SetState(WaveState.Pause);

            PrepareNextWave();
        }

        public void Tick(float dt)
        {
            if (_state.State == WaveState.Pause) return;

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
                        _spawner.Spawn(group.Data);
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

            if (_currWaveIndex >= _waves.Waves.Length)
            {
                _state.SetState(WaveState.Completed);
                return;
            }

            Wave wave = _waves.Waves[_currWaveIndex];

            _waveDelayTimer = _delayBeforeWave;
            _state.SetState(WaveState.Start);

            // TODO: implement IDebugger
            //Debug.Log($"Wave {_currWaveIndex + 1} will start in {_delayBeforeWave} sec");
        }


        private void StartWave()
        {
            Wave wave = _waves.Waves[_currWaveIndex];

            _activeGroups = new List<GroupRuntime>(wave.Groups.Length);

            foreach (var group in wave.Groups)
            {
                _activeGroups.Add(new GroupRuntime(group));
            }

            _state.SetState(WaveState.Spawning);

            // TODO: implement IDebugger
            //Debug.Log($"Wave {_currWaveIndex + 1} started");
        }
    }
}