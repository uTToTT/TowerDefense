using TToTT.TowerDefense.Enemies;
using TToTT.TowerDefense.Towers;
using TToTT.TowerDefense.UI;

public class GameLoop
{
    private readonly GameStateMachine _state;
    private readonly TickController _tick;
    private readonly EnemyManager _enemyManager;
    private readonly TowerManager _towerManager;
    private readonly ShopController _shopController;
    private readonly UIFlowController _uiFlowController;

    public GameLoop(
        GameStateMachine gameStateMachine,
        TickController tickController,
        EnemyManager enemyManager,
        TowerManager towerManager,
        ShopController shopController,
        UIFlowController uiFlowController)
    {
        _state = gameStateMachine;
        _tick = tickController;
        _enemyManager = enemyManager;
        _towerManager = towerManager;
        _shopController = shopController;
        _uiFlowController = uiFlowController;

        _tick.Register(_enemyManager);
        _tick.Register(_towerManager);
    }

    public void Tick(float dt) 
    {
        if (_state.State != GameState.WaveStarted) return;

        _tick.Tick(dt);
    }
}
