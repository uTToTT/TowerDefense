using TToTT.Core.DI;
using TToTT.Core.Installers;
using UnityEngine;

namespace TToTT.TowerDefense.Installers
{
    public class VFXInstaller : IInstaller
    {
        public VFXInstaller() { }

        public void Install(DIContainer container)
        {
            container.Bind<ParticlesGenerator>(Lifetime.Singleton);
            container.Bind<CameraShaker>(Lifetime.Singleton);
        }
    }
}