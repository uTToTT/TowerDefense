using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Level;

public class LevelInstaller : IInstaller
{
    private readonly LevelContext _ctx;

    public LevelInstaller(LevelContext ctx)
    {
        _ctx = ctx;
    }

    public void Install(DIContainer container)
    {
        container.BindInstance<LevelsRegistry>(_ctx.Levels);

        container.Bind<LevelLoader>(Lifetime.Singleton);
        container.Bind<LevelManager>(Lifetime.Singleton);
    }
}
