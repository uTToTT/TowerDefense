using TToTT.Core.DI;

namespace TToTT.Core.Installers
{
    public class GameInstaller : IInstaller
    {
        private readonly UIManager _uiManager;
        private readonly TowerManager _towerManager;
        private readonly MapManager _mapManager;
        private readonly EconomyManager _economyManager;
        private readonly EnemyManager _enemyManager;
        private readonly BuildManager _buildManager;
        private readonly ProductShop _productShop;
        private readonly ObjectSelector _objectSelector;
        private readonly ParticlesGenerator _particlesGenerator;
        private readonly CameraShaker _cameraShaker;

        public GameInstaller(
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
            _uiManager = uIManager;
            _towerManager = towerManager;
            _mapManager = mapManager;
            _economyManager = economyManager;
            _enemyManager = enemyManager;
            _buildManager = buildManager;
            _productShop = productShop;
            _objectSelector = objectSelector;
            _particlesGenerator = particlesGenerator;
            _cameraShaker = cameraShaker;
        }

        public void Install(DIContainer container)
        {
            container.Bind<PlayerInputController, PlayerInputController>(Lifetime.Singleton);

            /// 

            _uiManager.Init();

            _towerManager.Init();
            _mapManager.Init();
            _economyManager.Init();
            _enemyManager.Init();
            _buildManager.Init();
            _productShop.Init();
            _objectSelector.Init(container.Resolve<PlayerInputController>(), _mapManager);
            _particlesGenerator.Init();
            _cameraShaker.Init();

            ///

            container.BindInstance(_uiManager);
            container.BindInstance(_towerManager);
            container.BindInstance(_mapManager);
            container.BindInstance(_economyManager);
            container.BindInstance(_enemyManager);
            container.BindInstance(_buildManager);
            container.BindInstance(_productShop);
            container.BindInstance(_objectSelector);
            container.BindInstance(_particlesGenerator);
            container.BindInstance(_cameraShaker);

            container.Bind<GameLoop, GameLoop>();
        }
    }
}