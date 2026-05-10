using TToTT.Core.DI;
using TToTT.Core.Installers;

namespace TToTT.TowerDefense.Installers
{
    public class VFXInstaller : IInstaller
    {
        private readonly VFXContext _ctx;

        public VFXInstaller(VFXContext context) { _ctx = context; }

        public void Install(DIContainer container)
        {
            container.BindInstance<ParticlesFactoryRegistry>(_ctx.Particles);

            container.Bind<ParticlesGenerator>(Lifetime.Singleton);
            container.Bind<CameraShaker>(Lifetime.Singleton);
        }
    }
}