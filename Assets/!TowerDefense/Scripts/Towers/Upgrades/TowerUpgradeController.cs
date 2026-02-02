public class TowerUpgradeController
{
    private readonly Tower _tower;
    private readonly TowerUpgradeState _state;

    public UpgradeNodeConfig CurrentUpgrade {  get; private set; }
    public TowerUpgradeState State => _state;

    public TowerUpgradeController(Tower tower)
    {
        _tower = tower;
        _state = new TowerUpgradeState();
    }

    public bool CanPurchase(UpgradeNodeConfig node)
    {
        if (_state.IsPurchased(node))
            return false;

        //foreach (var condition in node.Conditions)
        //    if (!condition.IsSatisfied(_tower))
        //        return false;

        //return EconomyService.Instance.CanSpend(node.Cost);

        return true;
    }

    public void Purchase(UpgradeNodeConfig node)
    {
        if (!CanPurchase(node))
            return;

        //EconomyService.Instance.Spend(node.Cost);

        _tower.ApplyUpgrade(node);
        _state.MarkPurchased(node);
        CurrentUpgrade = node;
    }
}
