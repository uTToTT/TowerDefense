using System;

namespace TToTT.TowerDefense.Enemies
{
    public class WaveStateMachine
    {
        public event Action<WaveState> OnStateChanged;

        private WaveState _state;

        public WaveState State => _state;

        public void SetState(WaveState state)
        {
            if (state == _state) return;
            _state = state;
            OnStateChanged?.Invoke(state);
        }
    }
}