using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.UI;
using TToTT.TowerDefense.UI.Button;
using TToTT.TowerDefense.UI.Label;

namespace TToTT.TowerDefense.Installers
{
    public class UIInstaller : IInstaller
    {
        private readonly UIContext _ctx;

        public UIInstaller(UIContext ctx)
        {
            _ctx = ctx;
        }

        public void Install(DIContainer container)
        {
            container.BindInstance<MainMenuController>(_ctx.MainMenu);
            container.BindInstance<GameplayInterfaceController>(_ctx.Gameplay);
            container.BindInstance<UIWindowsController>(_ctx.WindowsController);
            container.BindInstance<LabelRegistry>(_ctx.Labels);
            container.BindInstance<ButtonRegistry>(_ctx.Buttons);

            container.Bind<UIFlowController>(Lifetime.Singleton);
        }
    }
}