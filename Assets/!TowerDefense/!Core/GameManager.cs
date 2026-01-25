using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MoveManager _moveManager;
    [SerializeField] private WaveController _waveController;
    [SerializeField] private TowerPlacer _towerPlacer;
    
    private PlayerInputController _playerInputController;

    private void Awake()
    {
        _playerInputController = new PlayerInputController();

        _playerInputController.Init();
        _playerInputController.EnableInput();

        _waveController.Init();
        _towerPlacer.Init();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        _moveManager.Tick(dt);
        _waveController.Tick(dt);

    }
}
