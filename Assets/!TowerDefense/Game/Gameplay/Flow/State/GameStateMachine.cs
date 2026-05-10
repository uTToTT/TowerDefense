using System;

namespace TToTT.TowerDefense.Gameloop
{
    public class GameStateMachine
    {
        public event Action<GameState> OnStateChanged;

        private readonly ILogger _logger;

        private GameState _state;

        public GameState State => _state;

        public GameStateMachine(ILogger logger)
        {
            _logger = logger;
        }

        public void SetState(GameState state)
        {
            if (state == _state) return;
            _state = state;
            _logger.Log($"Set [{_state}] state");
            OnStateChanged?.Invoke(state);
        }
    }
}