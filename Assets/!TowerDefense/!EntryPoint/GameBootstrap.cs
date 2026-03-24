using TToTT.Core.DI;
using TToTT.Core.Installers;

public class GameBootstrap 
{
    private DIContainer _container;

    public void Initialize()
    {
        _container = new DIContainer();

        new CoreInstaller().Install(_container);
        new GameInstaller().Install(_container);
    }
}
