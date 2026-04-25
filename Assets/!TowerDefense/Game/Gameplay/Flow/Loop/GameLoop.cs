public class GameLoop
{
    private readonly GameStateMachine _state;
    private readonly TickController _tick;

    public void Tick(float dt) 
    {
        if (_state.State != GameState.WaveStarted) return;

        _tick.Tick(dt);
    }
}
