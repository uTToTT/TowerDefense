using UnityEngine;

[CreateAssetMenu(menuName = "TD/Upgrade/Condition/Requires Node")]
public class RequiresUpgradeCondition : UpgradeCondition
{
    [SerializeField] private UpgradeNodeConfig _requiredNode;

    public override bool IsSatisfied(Tower tower) =>
        tower.UpgradeController.State.IsPurchased(_requiredNode);
}
