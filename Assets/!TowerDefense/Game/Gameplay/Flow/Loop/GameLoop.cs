public class GameLoop
{
    private readonly GameStateMachine _state;
    private readonly TickController _tick;

    public GameLoop(
        GameStateMachine gameStateMachine,
        TickController tickController)
    {
        _state = gameStateMachine;
        _tick = tickController;
    }

    public void Tick(float dt) 
    {
        if (_state.State != GameState.WaveStarted) return;

        _tick.Tick(dt);
    }
}
