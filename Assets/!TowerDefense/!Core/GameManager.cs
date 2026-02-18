using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HorizontalLine]
    [SerializeField] private Button _startWaveButton;

    [HorizontalLine]
    [SerializeField] private Camera _worldCamera;

    [HorizontalLine]
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private TowerManager _towerManager;
    [SerializeField] private ObjectSelector _cellSelector;
    [SerializeField] private EconomyService _economyService;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Player _player;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private BuildManager _buildManager;
    [SerializeField] private ProductShop _productShop;

    private PlayerInputController _playerInputController;

    public PlayerInputController PlayerInputController => _playerInputController;
    public TowerManager TowerManager => _towerManager;
    public BuildManager BuildManager => _buildManager;

    public Camera WorldCamera => _worldCamera;

    public static GameManager Instance { get; private set; }

    public bool IsBattle { get; private set; }

    private bool _isInit = false;

    private void Awake()
    {
        Instance = this;

        _playerInputController = new PlayerInputController();

        _uiManager.Init();

        _playerInputController.Init();
        _playerInputController.EnableInput();

        _cellSelector.Init(_playerInputController, _mapManager);

        _towerManager.Init();
        _mapManager.Init();
        _economyService.Init();
        _enemyManager.Init();
        _buildManager.Init();
        _productShop.Init();

        _startWaveButton.onClick.AddListener(PlayerStartWave);

        _isInit = true;
    }

    private void Update()
    {
        if (!_isInit) return;

        float dt = Time.deltaTime;

        if (IsBattle)
        {
            _enemyManager.Tick(dt);
        }

        _towerManager.Tick(dt);
        _mapManager.Tick(dt);
    }

    private void PlayerStartWave()
    {
        _enemyManager.WaveController.PlayerStartWave();
        UIManager.Instance.CloseWindow(WindowType.PreparingToWave);

        IsBattle = true;
    }

    public void WaveEnded()
    {
        _enemyManager.WaveController.StopWave();
        UIManager.Instance.OpenWindow(WindowType.PreparingToWave);

        IsBattle = false;
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
        UIManager.Instance.OpenWindow(WindowType.Victory);

        IsBattle = false;
    }

    private void GameDefeat()
    {
        UIManager.Instance.OpenWindow(WindowType.Defeat);

        IsBattle = false;
    }
}
