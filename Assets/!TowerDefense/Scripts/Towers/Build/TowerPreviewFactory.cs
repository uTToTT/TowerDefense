using UnityEngine;

[CreateAssetMenu(fileName = "TowerPreviewFactory", menuName = "TD/Tower/Tower Preview Factory")]

public class TowerPreviewFactory : FactoryBase<TowerPreview>
{
    public TowerType Type => Prefab.TowerType;
}
