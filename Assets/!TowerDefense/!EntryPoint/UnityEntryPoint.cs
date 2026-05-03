using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Installers;
using TToTT.TowerDefense.UI;
using UnityEngine;

public class UnityEntryPoint : MonoBehaviour
{
    [SerializeField] private UIWindowsController _uiWindowController;
    [SerializeField] private CellFactoryRegistry _cellFactoryRegistry;
    [SerializeField] private Grid _grid;
    [SerializeField] private ShopConfig _shopConfig;
    [SerializeField] private ProductSlot[] _productSlot;
    [SerializeField] private ButtonWrapper _reroll;
    [SerializeField] private WalletView _walletView;
    [SerializeField] private MapObjectFactoryRegistry _mapObjectFactoryRegistry;

    private GameLoop _gameLoop;

    private void Awake()
    {
        var container = new DIContainer();

        new CoreInstaller().Install(container);
        new UIInstaller(_uiWindowController, _walletView).Install(container);
        new GameInstaller(
            _cellFactoryRegistry,
            _grid,
            _shopConfig,
            _productSlot,
            _reroll,
            _mapObjectFactoryRegistry).Install(container);

        _gameLoop = container.Resolve<GameLoop>();
    }

    private void Update()
    {
        _gameLoop.Tick(Time.deltaTime);
    }
}
