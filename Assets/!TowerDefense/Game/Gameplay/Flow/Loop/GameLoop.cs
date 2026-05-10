using System;
using TToTT.TowerDefense.Enemies;
using TToTT.TowerDefense.Gameloop;
using TToTT.TowerDefense.Level;
using TToTT.TowerDefense.Map;
using TToTT.TowerDefense.Towers;
using TToTT.TowerDefense.UI;

public class GameLoop : IDisposable
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
    private readonly Player _player;

    #region Init

    public GameLoop(
       GameStateMachine gameStateMachine,
       TickController tickController,
       TowerManager towerManager,
       ShopController shopController,
       UIFlowController uiFlowController,
       MapManager mapManager,
       EnemyManager enemyManager,
       LevelManager levelManager,
       WaveController waveController,
       Player player)
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
        _player = player;

        InitGameStateHandlers();

        _waveController.OnAllWavesCompleted += Victory;
        _levelManager.OnLevelLoaded += HandleLevelLoaded;
        _player.OnPlayerDie += Defeat;

        _tick.Register(_enemyManager);
        _tick.Register(_towerManager);
        _tick.Register(_mapManager);
    }

    public void Dispose()
    {
        _waveController.OnAllWavesCompleted -= Victory;
        _levelManager.OnLevelLoaded -= HandleLevelLoaded;
        _player.OnPlayerDie -= Defeat;

        DisposeGameStateHandlers();
    }

    private void InitGameStateHandlers()
    {
        _state.OnStateChanged += HandleGameStateChanged;
        _state.OnStateChanged += _waveController.HandleGameStateChanged;
    }

    private void DisposeGameStateHandlers()
    {
        _state.OnStateChanged -= HandleGameStateChanged;
        _state.OnStateChanged -= _waveController.HandleGameStateChanged;
    }

    #endregion

    public void Tick(float dt)
    {
        _tick.Tick(dt);
    }

    private void HandleGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                break;
            case GameState.GameplayLoading:
                StartLevel(0);
                break;
            case GameState.Preparing:
                break;
            case GameState.Wave:
                break;
            case GameState.Victory:
                break;
            case GameState.Defeat:
                break;
        }
    }

    private void StartLevel(int index)
    {
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
        _state.SetState(GameState.Preparing);
    }

    private void Defeat()
    {
        _state.SetState(GameState.Defeat);
    }

    private void Victory()
    {
        _state.SetState(GameState.Victory);
    }
}