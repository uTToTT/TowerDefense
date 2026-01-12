using UnityEngine;
[CreateAssetMenu(fileName = "Shape", menuName = "TD/Tower/Shape")]
public class TowerShapeSO : ScriptableObject
{
    [SerializeField] private CellOffset[] _occupiedCells;

    public CellOffset[] OccupiedCells => _occupiedCells;
}
