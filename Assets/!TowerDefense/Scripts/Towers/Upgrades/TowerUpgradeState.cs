using System.Collections.Generic;

public class TowerUpgradeState
{
    private readonly HashSet<UpgradeNodeConfig> _purchased = new();

    public bool IsPurchased(UpgradeNodeConfig node)
        => _purchased.Contains(node);

    public void MarkPurchased(UpgradeNodeConfig node)
        => _purchased.Add(node);
}
