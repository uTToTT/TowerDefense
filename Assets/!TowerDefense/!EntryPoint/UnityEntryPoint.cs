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

    [HorizontalLine]
    [SerializeField] private Player _player;

    private GameLoop _gameLoop;

    private void Awake()
    {
        var container = new DIContainer();
        new CoreInstaller().Install(container);
        new UIInstaller(_ui).Install(container);
        new GameInstaller(_map, _shop, _player, _enemy, _level).Install(container);
        _gameLoop = container.Resolve<GameLoop>();
    }

    private void Update()
    {
        _gameLoop.Tick(Time.deltaTime);
    }
}
