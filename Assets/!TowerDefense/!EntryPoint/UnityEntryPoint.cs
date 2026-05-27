using NaughtyAttributes;
using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Installers;
using UnityEngine;

public class UnityEntryPoint : MonoBehaviour
{
    [SerializeField] private MapContext _map;
    [SerializeField] private ShopContext _shop;
    [SerializeField] private UIContext _ui;
    [SerializeField] private EnemyContext _enemy;
    [SerializeField] private LevelContext _level;
    [SerializeField] private VFXContext _vfx;
    [SerializeField] private SFXContext _sfx;
    [SerializeField] private MonetizationContext _monetization;

    [HorizontalLine]
    [SerializeField] private Camera _camera;

    private GameLoop _gameLoop;
    private ILogHandler _previousLogHandler;

    private void Awake()
    {
        _previousLogHandler = Debug.unityLogger.logHandler;
        Debug.unityLogger.logHandler = new CustomLogHandler();

        var container = new DIContainer();

        container.BindInstance<Camera>(_camera);

        new CoreInstaller().Install(container);
        new UIInstaller(_ui).Install(container);
        new GameInstaller(_map, _shop, _enemy, _level, _vfx, _sfx, _monetization).Install(container);
        _gameLoop = container.Resolve<GameLoop>();
    }

    private void Update()
    {
        _gameLoop.Tick(Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (_previousLogHandler != null)
            Debug.unityLogger.logHandler = _previousLogHandler;
    }
}
