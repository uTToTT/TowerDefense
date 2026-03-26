using TToTT.Core.DI;
using TToTT.Core.Installers;

public class GameBootstrap
{
    private DIContainer _container;

    public void Initialize(
        UIManager uIManager,
        TowerManager towerManager,
        MapManager mapManager,
        EconomyManager economyManager,
        EnemyManager enemyManager,
        BuildManager buildManager,
        ProductShop productShop,
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
    }

    public T Resolve<T>() => _container.Resolve<T>();
}
