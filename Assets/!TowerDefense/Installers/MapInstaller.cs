using TToTT.Core.DI;
using TToTT.Core.Installers;
using TToTT.TowerDefense.Map;

namespace TToTT.TowerDefense.Installers
{
    public class MapInstaller : IInstaller
    {
        private readonly CellFactoryRegistry _cellFactoryRegistry;

        public MapInstaller(CellFactoryRegistry cellFactoryRegistry)
        {
            _cellFactoryRegistry = cellFactoryRegistry;
        }

        public void Install(DIContainer container)
        {
            container.BindInstance(_cellFactoryRegistry);
            container.Bind<MapComposer, MapComposer>(Lifetime.Singleton);

            container.Bind<MapManager, MapManager>(Lifetime.Singleton);
        }
    }
}