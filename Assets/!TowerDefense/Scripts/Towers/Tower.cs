using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[RequireComponent(typeof(TargetingModule))]
public class Tower : MonoBehaviour, IPoolable, IEntityLifecycle
{
    [SerializeField] protected TowerType _towerType;
    [SerializeField] private TowerShapeSO _shape;
    [HorizontalLine]

    [HorizontalLine] private UpgradeNodeConfig _upgradeTree;

    private TowerUpgradeController _upgradeController;
    private TargetingModule _targetingModule;
    private readonly HashSet<ITowerModule> _modules = new();

    public UpgradeNodeConfig UpgradeTree => _upgradeTree;
    public TowerUpgradeController UpgradeController => _upgradeController;
    public TargetingModule TargetingModule => _targetingModule;

    public void Initialize()
    {
        _targetingModule = GetComponent<TargetingModule>();
        AddModule(_targetingModule);
        _upgradeController = new TowerUpgradeController(this);
    }

    public void ApplyUpgrade(UpgradeNodeConfig config)
    {
        foreach (var moduleConfig in config.Modules)
        {
            var module = TowerModuleFactory.Create(moduleConfig, this);
            AddModule(module);
        }
    }

    private void BindOnHitEffects()
    {
        var attack = _modules.OfType<AttackModule>().FirstOrDefault();
        if (attack == null)
            return;

        foreach (var module in _modules)
        {
            if (module is IOnHitEffect effect)
            {
                attack.RegisterOnHitEffect(effect);
            }
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