using TToTT.TowerDefense.UI;
using UnityEngine;

public class UnityEntryPoint : MonoBehaviour
{
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private TowerManager _towerManager;
    [SerializeField] private ObjectSelector _cellSelector;
    [SerializeField] private EconomyManager _economyManager;
    [SerializeField] private UIFlowController _uiManager;
    [SerializeField] private Player _player;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private BuildManager _buildManager;
    [SerializeField] private ProductShop _productShop;
    [SerializeField] private ParticlesGenerator _particlesGenerator;
    [SerializeField] private CameraShaker _cameraShaker;

    private GameBootstrap _bootstrap;


    private void Awake()
    {
        _bootstrap = new GameBootstrap();

        _bootstrap.Initialize(
            _uiManager,
            _towerManager,
            _mapManager,
            _economyManager,
            _enemyManager,
            _buildManager,
            _productShop,
            _cellSelector,
            _particlesGenerator,
            _cameraShaker);

        StartGame();
    }

    private void StartGame()
    {

    }
}
