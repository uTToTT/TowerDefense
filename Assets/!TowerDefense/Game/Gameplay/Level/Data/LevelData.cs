using UnityEngine;

namespace TToTT.TowerDefense.Level
{
    [CreateAssetMenu(fileName = "Level", menuName = "TD/Level/Level")]
    public class LevelData : ScriptableObject
    {
        public int Level;
        public MapData Map;
        public WavesData Waves;
    }
}