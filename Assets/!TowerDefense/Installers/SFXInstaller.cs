using TToTT.Core.DI;
using TToTT.Core.Installers;

public class SFXInstaller : IInstaller
{
    private readonly SFXContext _ctx;

    public SFXInstaller(SFXContext context) { _ctx = context; }

    public void Install(DIContainer container)
    {
        container.BindInstance<SoundRegistry>(_ctx.Sounds);   
        container.BindInstance<AudioPlayerFactory>(_ctx.PlayerFactory);
        
        container.Bind<AudioService>(Lifetime.Singleton);
    }
}
