namespace TToTT.TowerDefense.Map
{
    public class MapLoader
    {
        private readonly MapRegistry _registry;

        public MapLoader(MapRegistry registry)
        {
            _registry = registry;
        }

        public bool TryLoad(int index, out MapData map)
        {
            if (!_registry.TryGetMap(index, out map))
            {
                // TODO: IDebugger
                throw new System.Exception($"Map [{index}] not found in registry");
            }
            return true;
        }
    }
}