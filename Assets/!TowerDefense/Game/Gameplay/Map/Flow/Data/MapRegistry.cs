using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    [CreateAssetMenu(fileName = "MapRegistry", menuName = "TD/Map/Map Registry")]
    public class MapRegistry : ScriptableObject
    {
        [SerializeField] private MapData[] _maps;

        public bool TryGetMap(int index, out MapData map)
        {
            if (index < 0 || index >= _maps.Length)
            {
                map = null;
                return false;
            }

            map = _maps[index];
            return true;
        }

        public int Count => _maps.Length;
    }
}