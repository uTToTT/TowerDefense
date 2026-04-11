using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Map;
using TToTT.TowerDefense.UI;

public class GameBootstrap
{
    private DIContainer _container;

    public void Initialize(
        UIFlowController uIManager,
        TowerManager towerManager,
        MapManager mapManager,
        EconomyController economyManager,
        EnemyManager enemyManager,
        BuildManager buildManager,
        ProductShopController productShop,
        ObjectSelector objectSelector,
        ParticlesGenerator particlesGenerator,
        CameraShaker cameraShaker)
    {
        _container = new DIContainer();
        var gameInstaller = new GameInstaller(
            uIManager,
            towerManager,
            mapManager,
            economyManager,
            enemyManager,
            buildManager,
            productShop,
            objectSelector,
            particlesGenerator,
            cameraShaker);

        new CoreInstaller().Install(_container);
        gameInstaller.Install(_container);
        new UIInstaller().Install(_container);
    }

    public T Resolve<T>() => _container.Resolve<T>();
}
