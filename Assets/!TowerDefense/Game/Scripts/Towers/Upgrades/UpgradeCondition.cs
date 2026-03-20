using UnityEngine;

public abstract class UpgradeCondition : ScriptableObject
{
    public abstract bool IsSatisfied(Tower tower);
}
