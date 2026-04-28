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

    private GameLoop _gameLoop;

    private void Awake()
    {
        var container = new DIContainer();

        new CoreInstaller().Install(container);
        new GameInstaller(_cellFactoryRegistry, _grid).Install(container);
        new UIInstaller(_uiWindowController).Install(container);

        _gameLoop = container.Resolve<GameLoop>();
    }

    private void Update()
    {
        _gameLoop.Tick(Time.deltaTime);
    }
}
