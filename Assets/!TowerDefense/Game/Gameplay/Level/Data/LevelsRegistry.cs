using UnityEngine;

namespace TToTT.TowerDefense.Level
{
    [CreateAssetMenu(fileName = "LevelsRegistry", menuName = "TD/Level/Levels Registry")]
    public class LevelsRegistry : ScriptableObject
    {
        [SerializeField] private LevelData[] _levels;

        public bool TryGetLevel(int index, out LevelData level)
        {
            if (index < 0 || index >= _levels.Length)
            {
                level = null;
                return false;
            }

            level = _levels[index];
            return true;
        }

        public int Count => _levels.Length;
    }
}