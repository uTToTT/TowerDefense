using TToTT.Core.DI;

namespace TToTT.Core.Installers
{
    public interface IInstaller
    {
        void Install(DIContainer container);
    }
}
