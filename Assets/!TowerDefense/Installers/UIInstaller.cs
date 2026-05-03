using TToTT.Core.DI;
using TToTT.Core.Installers;

namespace TToTT.TowerDefense.UI
{
    public class UIInstaller : IInstaller
    {
        private readonly UIWindowsController _windowsController;
        private readonly IWalletView _walletView;

        public UIInstaller(
            UIWindowsController windowsController,
            IWalletView walletView)
        {
            _windowsController = windowsController;
            _walletView = walletView;
        }

        public void Install(DIContainer container)
        {
            container.BindInstance(_windowsController);
            container.BindInstance<IWalletView>(_walletView);

            container.Bind<UIFlowController>(Lifetime.Singleton);
        }
    }
}