using System;

public class GameStateMachine
{
    public event Action<GameState> OnStateChanged;

    private GameState _state;

    public GameState State => _state;

    public void SetState(GameState state)
    {
        if (state == _state) return; 
        _state = state;
        OnStateChanged?.Invoke(state);
    }
}
