using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private MoveManager _moveManager;
    [SerializeField] private WaveController _waveController;
    [SerializeField] private TowerPlacer _towerPlacer;
    [SerializeField] private CellSelector _cellSelector;

    private PlayerInputController _playerInputController;

    public PlayerInputController PlayerInputController => _playerInputController;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        _playerInputController = new PlayerInputController();

        _playerInputController.Init();
        _playerInputController.EnableInput();

        _cellSelector.Init();
        _playerInputController.OnTapPerformed += _cellSelector.OnTap;

        _waveController.Init();
        _towerPlacer.Init();
        _mapManager.Init();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        _moveManager.Tick(dt);
        _waveController.Tick(dt);

    }
}
