using UnityEngine;
[CreateAssetMenu(fileName = "Shape", menuName = "TD/Map/Shape")]
public class MapObjectShape : ScriptableObject
{
    [SerializeField] private CellOffset[] _occupiedCells;
    [SerializeField] private MapObjectPort[] _ports;

    public CellOffset[] OccupiedCells => _occupiedCells;
    public MapObjectPort[] Ports => _ports;
}
