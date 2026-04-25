using TToTT.Core.DI;
using TToTT.Core.Installers;

namespace TToTT.TowerDefense.UI
{
    public class UIInstaller : IInstaller
    {
        private readonly UIWindowsController _windowsController;

        public UIInstaller(UIWindowsController windowsController)
        {
            _windowsController = windowsController;
        }

        public void Install(DIContainer container)
        {
            container.BindInstance(_windowsController);

            container.Bind<UIFlowController>(Lifetime.Singleton);
        }
    }
}