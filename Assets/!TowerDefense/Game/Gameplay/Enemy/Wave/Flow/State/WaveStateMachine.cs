using System;

namespace TToTT.TowerDefense.Enemies
{
    public class WaveStateMachine
    {
        public event Action<WaveState> OnStateChanged;

        private readonly ILogger _logger;

        private WaveState _state;

        public WaveState State => _state;

        public WaveStateMachine(ILogger logger)
        {
            _logger = logger;
        }

        public void SetState(WaveState state)
        {
            if (state == _state) return;
            _state = state;
            //_logger.Log($"Set [{_state}] wave state");
            OnStateChanged?.Invoke(state);
        }
    }
}