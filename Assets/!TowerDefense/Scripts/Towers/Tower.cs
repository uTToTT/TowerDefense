using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, IPoolable, IEntityLifecycle
{
    [SerializeField] protected TowerType _towerType;
    [SerializeField] private TowerShapeSO _shape;
    [HorizontalLine]

    [HorizontalLine] private UpgradeNodeConfig _upgradeTree;

    private TowerUpgradeController _upgradeController;

    private readonly HashSet<ITowerModule> _modules = new();
    public UpgradeNodeConfig UpgradeTree => _upgradeTree;
    public TowerUpgradeController UpgradeController => _upgradeController;

    public void Initialize(UpgradeNodeConfig config)
    {
        _upgradeController = new TowerUpgradeController(this);

        foreach (var moduleConfig in config.Modules)
        {
            var module = TowerModuleFactory.Create(moduleConfig, this);
            AddModule(module);
        }
    }

    public void AddModule(ITowerModule module)
    {
        if (!_modules.Add(module))
        {
            Debug.Log($"Module already exists [{module.GetType()}]");
        }
    }

    public void ApplyConfig(TowerModuleConfig config)
    {
        foreach (var module in _modules)
            module.TryApplyConfig(config);
    }

    public void Tick()
    {
        foreach (var module in _modules)
            module.Tick(Time.deltaTime);
    }

    public TowerShapeSO Shape => _shape;
    public TowerType TowerType => _towerType;
    public Grid Grid { get; set; }
    public bool IsActive { get; set; }

    public int GetSpecPrice(int index) => 0;

    public void SetUniqueTowerIndex(int index)
    {
    }

    public void Dispose()
    {
    }

    public void OnPreload()
    {
    }

    public void OnActivated()
    {
    }

    public void OnDeactivated()
    {
    }

    public void OnReturned()
    {
    }

    public void OnDestroyed()
    {
    }
}

public enum SpecTypeMinigun
{
    None,
    Explosion,
    Freeze,
}

public enum SpecTypeTwiin
{
    None,
    TwoToOneAtack,
    Shard,
}

public enum SpecTypeGravity
{
    None,
    MoneyMultyplier,
    HpDivisor,
}

public enum SpecTypeRail
{
    None,
    Critical,
    BreakArmor,
}