using UnityEngine;

[CreateAssetMenu(fileName = "CellFactory", menuName = "TD/Map/Cell Factory")]
public class CellFactory : FactoryBase<Cell>
{
    public CellType CellType => Prefab.CellType;
}
