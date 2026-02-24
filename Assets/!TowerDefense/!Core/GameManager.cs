using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public event Action OnGameVictory;
    public event Action OnGameDefeat;
    public event Action OnGameRestart;
    public event Action OnWaveEnded;
    public event Action OnWaveStarted;

    [HorizontalLine]
    [SerializeField] private Button _startWaveButton;

    [HorizontalLine]
    [SerializeField] private UnityEngine.Camera _worldCamera;

    [HorizontalLine]
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private TowerManager _towerManager;
    [SerializeField] private ObjectSelector _cellSelector;
    [SerializeField] private EconomyManager _economyService;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Player _player;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private BuildManager _buildManager;
    [SerializeField] private ProductShop _productShop;
    [SerializeField] private ParticlesGenerator _particlesGenerator;
    [SerializeField] private CameraShaker _cameraShaker;

    private PlayerInputController _playerInputController;

    public PlayerInputController PlayerInputController => _playerInputController;
    public TowerManager TowerManager => _towerManager;
    public BuildManager BuildManager => _buildManager;
    public UnityEngine.Camera WorldCamera => _worldCamera;

    public static GameManager Instance { get; private set; }

    public bool IsBattle { get; private set; }

    private bool _isInit = false;
    private float _timeModifier = 1f;
    private bool _timeFreezed = false;

    private void Awake()
    {
        InitDependencies();
        SetData();
        StartGame();
    }

    private void Update()
    {
        if (!_isInit) return;
        if (_timeFreezed) return;

        float dt = Time.deltaTime * _timeModifier;

        if (IsBattle)
        {
            _enemyManager.Tick(dt);
        }

        if (!IsBattle)
        {
            _cellSelector.Tick(dt);
        }

        _towerManager.Tick(dt);
        _mapManager.Tick(dt);
    }

    private void InitDependencies()
    {
        Instance = this;

        _playerInputController = new PlayerInputController();

        _uiManager.Init();

        _playerInputController.Init();
        _playerInputController.EnableInput();

        _towerManager.Init();
        _mapManager.Init();
        _economyService.Init();
        _enemyManager.Init();
        _buildManager.Init();
        _productShop.Init();
        _cellSelector.Init(_playerInputController, _mapManager);
        _particlesGenerator.Init();
        _cameraShaker.Init();

        _startWaveButton.onClick.AddListener(PlayerStartWave);

        _isInit = true;
    }

    private void SetData()
    {
    }

    #region Time

    public void StopTime() { _timeFreezed = true; }
    public void StartTime() { _timeFreezed = false; }
    public void SetTimeModifier(float mod) { _timeModifier = Mathf.Max(0, mod); }

    #endregion

    #region Game cycle

    public void StartGame()
    {
        _uiManager.CloseAllWindows();
        _uiManager.OpenWindow(WindowType.Main);

    }

    public void RestartGame()
    {
        StopTime();

        _towerManager.Restart();
        _economyService.Restart();
        _player.Restart();
        _enemyManager.Restart();
        _particlesGenerator.Restart();

        StartTime();

        OnGameRestart?.Invoke();
    }

    private void PlayerStartWave()
    {
        _enemyManager.WaveController.PlayerStartWave();
        UIManager.Instance.CloseWindow(WindowType.Gameplay);

        IsBattle = true;
    }

    public void WaveEnded()
    {
        _enemyManager.WaveController.StopWave();
        UIManager.Instance.OpenWindow(WindowType.Gameplay);
        _productShop.Reroll();

        IsBattle = false;
        OnWaveEnded?.Invoke();
    }

    public void AllWavesEnded()
    {
        //Debug.Log("All waves completed");
        if (_player.CurrHP > 0)
        {
            GameVictory();
        }

        IsBattle = false;
    }

    public void PlayerBaseDestroyed()
    {
        GameDefeat();

        IsBattle = false;
    }

    private void GameVictory()
    {
        _enemyManager.WaveController.StopWave();

        IsBattle = false;
        OnGameVictory?.Invoke();
    }

    private void GameDefeat()
    {
        IsBattle = false;
        OnGameDefeat?.Invoke();
    }

    #endregion
}
