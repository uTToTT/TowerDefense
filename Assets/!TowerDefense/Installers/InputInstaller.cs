using TToTT.Core.DI;
using TToTT.Core.Installers;

namespace TToTT.TowerDefense.Installers
{
    public class InputInstaller : IInstaller
    {
        public InputInstaller()
        {

        }

        public void Install(DIContainer container)
        {
            container.Bind<PlayerInputController>(Lifetime.Singleton);
            container.Bind<DragAndDropController>(Lifetime.Singleton);
            container.Bind<PlayerInputController>(Lifetime.Singleton);
            container.Bind<ObjectSelector>(Lifetime.Singleton);
        }
    }
}