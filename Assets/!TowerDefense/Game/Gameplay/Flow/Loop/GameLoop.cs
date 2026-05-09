using TToTT.TowerDefense.Enemies;
using TToTT.TowerDefense.Level;
using TToTT.TowerDefense.Map;
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
    private readonly MapManager _mapManager;
    private readonly LevelManager _levelManager;
    private readonly WaveController _waveController;

    public GameLoop(
        GameStateMachine gameStateMachine,
        TickController tickController,
        TowerManager towerManager,
        ShopController shopController,
        UIFlowController uiFlowController,
        MapManager mapManager,
        EnemyManager enemyManager,
        LevelManager levelManager,
        WaveController waveController)
    {
        _state = gameStateMachine;
        _tick = tickController;
        _enemyManager = enemyManager;
        _towerManager = towerManager;
        _shopController = shopController;
        _uiFlowController = uiFlowController;
        _mapManager = mapManager;
        _levelManager = levelManager;
        _waveController = waveController;

        _levelManager.OnLevelLoaded += HandleLevelLoaded;

        _tick.Register(_enemyManager);
        _tick.Register(_towerManager);
        _tick.Register(_mapManager);

        StartLevel(0);
    }

    public void Tick(float dt)
    {
        _tick.Tick(dt);
    }

    private void StartLevel(int index)
    {
        _state.SetState(GameState.Pause);
        _mapManager.Restart();
        _towerManager.Restart();
        _shopController.Restart();
        _enemyManager.Restart();

        _levelManager.TryLoadLevel(index); 
    }

    private void HandleLevelLoaded(LevelData level)
    {
        _mapManager.TryBuildMap(level);
        _waveController.InitData(level.Waves);
        _state.SetState(GameState.WaveStarted);
    }
}