using UnityEngine;

[CreateAssetMenu(fileName = "TowerFactory", menuName = "TD/Tower/Tower Factory")]
public class TowerFactory : FactoryBase<Tower>
{
    public TowerType Type => Prefab.TowerType;
}
