using TToTT.TowerDefense.Level;

namespace TToTT.TowerDefense.Map
{
    public class MapLoader
    {
        public bool TryLoad(LevelData levelData, out MapData map)
        {
            if (levelData.Map == null)
            {
                // TODO: IDebugger
                throw new System.Exception($"Map in Level [{levelData.Level}] not found in registry");
            }

            map = levelData.Map;
            return true;
        }
    }
}