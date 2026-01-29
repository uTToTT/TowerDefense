using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HorizontalLine]
    [SerializeField] private Button _startWaveButton;

    [SerializeField] private MapManager _mapManager;
    [SerializeField] private MoveManager _moveManager;
    [SerializeField] private WaveController _waveController;
    [SerializeField] private TowerManager _towerManager;
    [SerializeField] private CellSelector _cellSelector;
    [SerializeField] private EconomyService _economyService;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Player _player;

    private PlayerInputController _playerInputController;

    public PlayerInputController PlayerInputController => _playerInputController;
    public TowerManager TowerManager => _towerManager;

    public static GameManager Instance { get; private set; }

    private bool _isBattle;

    private void Awake()
    {
        Instance = this;

        _playerInputController = new PlayerInputController();

        _uiManager.Init();

        _playerInputController.Init();
        _playerInputController.EnableInput();

        _cellSelector.Init();
        _playerInputController.OnTapPerformed += _cellSelector.OnTapPerformed;
        _playerInputController.OnTapCanceled += _cellSelector.OnTapCanceled;

        _waveController.Init();
        _towerManager.Init();
        _mapManager.Init();
        _economyService.Init();

        _waveController.OnMoneyDropped += _economyService.AddMoney;
        _startWaveButton.onClick.AddListener(PlayerStartWave);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (_isBattle)
        {
            _moveManager.Tick(dt);
            _waveController.Tick(dt);
        }

        _towerManager.Tick(dt);
    }

    private void PlayerStartWave()
    {
        _waveController.PlayerStartWave();
        UIManager.Instance.CloseWindow(WindowType.PreparingToWave);

        _isBattle = true;
    }

    public void WaveEnded()
    {
        _waveController.StopWave();
        UIManager.Instance.OpenWindow(WindowType.PreparingToWave);

        _isBattle = false;
    }

    public void AllWavesEnded()
    {
        Debug.Log("All waves completed");
        if (_player.CurrHP > 0)
        {
            GameVictory();
        }

        _isBattle = false;
    }

    public void PlayerBaseDestroyed()
    {
        GameDefeat();

        _isBattle = false;
    }

    private void GameVictory()
    {
        _waveController.StopWave();
        UIManager.Instance.OpenWindow(WindowType.Victory);

        _isBattle = false;
    }

    private void GameDefeat()
    {
        UIManager.Instance.OpenWindow(WindowType.Defeat);

        _isBattle = false;
    }
}
