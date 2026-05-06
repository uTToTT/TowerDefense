using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Enemies;

namespace TToTT.TowerDefense.Installers
{
    public class EnemyInstaller : IInstaller
    {
        private readonly EnemyContext _ctx;

        public EnemyInstaller(EnemyContext ctx) { _ctx = ctx; }

        public void Install(DIContainer container)
        {
            // Configs
            container.BindInstance(_ctx.Factory);

            // Wave
            container.Bind<WaveStateMachine>(Lifetime.Singleton);
            container.Bind<WaveController>(Lifetime.Singleton);

            // Spawn
            container.Bind<EnemySpawner>(Lifetime.Singleton);

            // Enemy pipeline
            container.Bind<EnemyManager>(Lifetime.Singleton);
        }
    }
}