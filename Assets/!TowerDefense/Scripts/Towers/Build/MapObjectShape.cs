using UnityEngine;
[CreateAssetMenu(fileName = "Shape", menuName = "TD/Map/Shape")]
public class MapObjectShape : ScriptableObject
{
    [SerializeField] private CellOffset[] _occupiedCells;

    public CellOffset[] OccupiedCells => _occupiedCells;
}
