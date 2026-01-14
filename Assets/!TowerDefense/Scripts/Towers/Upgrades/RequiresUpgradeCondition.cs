using UnityEngine;

[CreateAssetMenu(menuName = "TD/Upgrade/Condition/Requires Node")]
public class RequiresUpgradeCondition : UpgradeCondition
{
    public UpgradeNodeConfig RequiredNode;

    public override bool IsSatisfied(Tower tower)
    {
        return tower.UpgradeController.State.IsPurchased(RequiredNode);
    }
}
