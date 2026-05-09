using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Towers;

namespace TToTT.TowerDefense.Installers
{
    public class GameInstaller : IInstaller
    {
        private readonly MapContext _mapCtx;
        private readonly ShopContext _shopCtx;
        private readonly Player _player;
        private readonly EnemyContext _enemyCtx;
        private readonly LevelContext _levelCtx;

        public GameInstaller(
            MapContext mapContext,
            ShopContext shopCtx,
            Player player,
            EnemyContext enemyCtx,
            LevelContext levelCtx)
        {
            _player = player;
            _mapCtx = mapContext;
            _shopCtx = shopCtx;
            _player = player;
            _enemyCtx = enemyCtx;
            _levelCtx = levelCtx;
        }

        public void Install(DIContainer container)
        {
            new InputInstaller().Install(container);
            new VFXInstaller().Install(container);
            new MapInstaller(_mapCtx).Install(container);
            new EconomyInstaller(_shopCtx).Install(container);
            new EnemyInstaller(_enemyCtx).Install(container);
            new LevelInstaller(_levelCtx).Install(container);

            container.BindInstance<IPlayerTarget>(_player);
            container.Bind<TowerManager>(Lifetime.Singleton);
            container.Bind<GameStateMachine>(Lifetime.Singleton);
            container.Bind<GameLoop>(Lifetime.Singleton);

            container.Bind<ILogger, Logger>();
        }
    }
}