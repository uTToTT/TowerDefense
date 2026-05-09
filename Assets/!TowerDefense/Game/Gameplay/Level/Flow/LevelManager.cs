using System;

namespace TToTT.TowerDefense.Level
{
    public class LevelManager 
    {
        public event Action<LevelData> OnLevelLoaded;

        private readonly LevelLoader _levelLoader;
        private LevelData _current;

        public LevelManager(LevelLoader levelLoader)
        {
            _levelLoader = levelLoader;
        }

        public bool TryLoadLevel(int index)
        {
            if(!_levelLoader.TryLoadLevel(index, out var level)) return false;

            _current = level;
            OnLevelLoaded(level);
            return true;
        }
    }
}