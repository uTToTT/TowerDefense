using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Gameloop;
using TToTT.TowerDefense.Towers;

namespace TToTT.TowerDefense.Installers
{
    public class GameInstaller : IInstaller
    {
        private readonly MapContext _mapCtx;
        private readonly ShopContext _shopCtx;
        private readonly EnemyContext _enemyCtx;
        private readonly LevelContext _levelCtx;
        private readonly VFXContext _vfxContext;
        private readonly SFXContext _sfxContext;

        public GameInstaller(
            MapContext mapContext,
            ShopContext shopCtx,
            EnemyContext enemyCtx,
            LevelContext levelCtx,
            VFXContext vfxContext,
            SFXContext sfxContext)
        {
            _mapCtx = mapContext;
            _shopCtx = shopCtx;
            _enemyCtx = enemyCtx;
            _levelCtx = levelCtx;
            _vfxContext = vfxContext;
            _sfxContext = sfxContext;
        }

        public void Install(DIContainer container)
        {
            new InputInstaller().Install(container);
            new VFXInstaller(_vfxContext).Install(container);
            new SFXInstaller(_sfxContext).Install(container);
            new MapInstaller(_mapCtx).Install(container);
            new EconomyInstaller(_shopCtx).Install(container);
            new EnemyInstaller(_enemyCtx).Install(container);
            new LevelInstaller(_levelCtx).Install(container);
            new MonetizationInstaller().Install(container);

            container.Bind<Player>(Lifetime.Singleton);
            container.BindFactory<IPlayerTarget>(
                c => c.Resolve<Player>(),
                Lifetime.Singleton);

            container.Bind<TowerManager>(Lifetime.Singleton);
            container.Bind<GameStateMachine>(Lifetime.Singleton);
            container.Bind<GameLoop>(Lifetime.Singleton);

            container.Bind<ILogger, Logger>();
        }
    }
}