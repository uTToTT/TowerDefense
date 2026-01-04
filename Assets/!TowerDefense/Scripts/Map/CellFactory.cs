using UnityEngine;

[CreateAssetMenu(fileName = "CellFactory", menuName = "Map/Cell Factory")]
public class CellFactory : FactoryBase<Cell>
{
    public CellType CellType => Prefab.CellType;
}
