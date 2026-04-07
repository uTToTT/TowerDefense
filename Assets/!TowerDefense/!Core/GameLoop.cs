using NaughtyAttributes;
using System;
using TToTT.TowerDefense.Map;
using TToTT.TowerDefense.UI;
using UnityEngine;

public class GameLoop // TODO: separate
{
    public event Action OnGameVictory;
    public event Action OnGameDefeat;
    public event Action OnGameRestart;
    public event Action OnWaveEnded;
    public event Action OnWaveStarted;

    [HorizontalLine]
    [SerializeField] private ButtonWrapper _startWaveButton; // replace to UI

    [HorizontalLine]
    [SerializeField] private UnityEngine.Camera _worldCamera;

    private readonly MapManager _mapManager;
    private readonly TowerManager _towerManager;
    private readonly ObjectSelector _cellSelector;
    private readonly EconomyController _economyService;
    private readonly UIFlowController _uiManager;
    private readonly Player _player;
    private readonly EnemyManager _enemyManager;
    private readonly BuildManager _buildManager;
    private readonly ProductShop _productShop;
    private readonly ParticlesGenerator _particlesGenerator;

    public TowerManager TowerManager => _towerManager;
    public BuildManager BuildManager => _buildManager;

    public bool IsBattle { get; private set; }

    public void Tick(float dt) // replace to TickController
    {
        if (IsBattle)
            _enemyManager.Tick(dt);

        if (!IsBattle)
            _cellSelector.Tick(dt);

        _towerManager.Tick(dt);
        _mapManager.Tick(dt);
    }

    #region Game cycle

    public void Start()
    {
        Restart();

        _uiManager.OpenMain();
    }

    public void Restart()
    {
        //StopTime();

        _mapManager.Restart();
        _towerManager.Restart();
        _economyService.Restart();
        _player.Restart();
        _enemyManager.Restart();
        _particlesGenerator.Restart();
        _productShop.Restart();

        //StartTime();

        OnGameRestart?.Invoke();
    }

    private void PlayerStartWave()
    {
        _enemyManager.WaveController.PlayerStartWave();
        UIFlowController.Instance.CloseWindow(WindowType.Gameplay); // ref

        IsBattle = true;
    }

    public void WaveEnded()
    {
        _enemyManager.WaveController.StopWave();
        UIFlowController.Instance.OpenWindow(WindowType.Gameplay); // ref
        _productShop.Reroll();

        IsBattle = false;
        OnWaveEnded?.Invoke();
    }

    public void AllWavesEnded()
    {
        //Debug.Log("All waves completed"); // requried ILooger
        if (_player.CurrHP > 0)
        {
            Victory();
        }

        IsBattle = false;
    }

    public void PlayerBaseDestroyed()
    {
        Defeat();

        IsBattle = false;
    }

    private void Victory()
    {
        _enemyManager.WaveController.StopWave();

        IsBattle = false;
        OnGameVictory?.Invoke();
    }

    private void Defeat()
    {
        IsBattle = false;
        OnGameDefeat?.Invoke();
    }

    #endregion
}
