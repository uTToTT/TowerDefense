using System;

namespace TToTT.TowerDefense.Level
{
    public class LevelLoader
    {
        private readonly LevelsRegistry _levels;

        public LevelLoader(LevelsRegistry levelsRegistry)
        {
            _levels = levelsRegistry;
        }

        public bool TryLoadLevel(int index, out LevelData level)
        {
            if (!_levels.TryGetLevel(index, out level)) 
                throw new Exception($"Level [{index}] not found");

            return true;
        }
    }
}