using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TargetingModule))]
public class Tower : MonoBehaviour, IPoolable, IEntityLifecycle, IMapObject
{
    [SerializeField] protected TowerType _towerType;
    [SerializeField] private MapObjectShape _shape;
    [HorizontalLine]

    [SerializeField] private UpgradeNodeConfig _upgradeTree;

    private TowerUpgradeController _upgradeController;
    private TargetingModule _targetingModule;
    private readonly HashSet<ITowerModule> _modules = new();

    public UpgradeNodeConfig UpgradeTree => _upgradeTree;
    public TowerUpgradeController UpgradeController => _upgradeController;
    public TargetingModule TargetingModule => _targetingModule;

    public bool IsEnabled { get; private set; }

    public void Enable() => IsEnabled = true;
    public void Disable() => IsEnabled = false;

    public void ApplyUpgrade(UpgradeNodeConfig config)
    {
        bool modulesChanged = false;

        foreach (var moduleConfig in config.AddModuleConfigs)
        {
            if (HasModule(moduleConfig.ModuleType))
                continue;

            var module = TowerModuleFactory.Create(moduleConfig, this);
            AddModule(module);
            modulesChanged = true;
        }

        foreach (var moduleConfig in config.ModifyModuleConfigs)
        {
            ApplyConfig(moduleConfig);
            //Debug.Log($"Apply {moduleConfig.GetType()}");
        }

        if (modulesChanged)
        {
            BindOnHitEffects();
        }
    }

    private bool HasModule(ModuleType moduleType)
    {
        foreach (var module in _modules)
            if (module.ModuleType == moduleType)
                return true;

        return false;
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

    private void AddModule(ITowerModule module)
    {
        if (!_modules.Add(module))
        {
            Debug.Log($"Module already exists [{module.GetType()}]");
        }
    }

    private void ApplyConfig(TowerModuleConfig config)
    {
        if (config is TargetingModuleConfig)
        {
            _targetingModule.TryApplyConfig(config);
            return;
        }

        foreach (var module in _modules)
            module.TryApplyConfig(config);
    }

    private void ClearModules()
    {
        _modules.Clear();
    }

    public void Tick(float dt)
    {
        if (!IsEnabled)
            return;

        foreach (var module in _modules)
            module.Tick(dt);
    }

    public Transform Transform => transform;
    public Vector2Int MapPos { get;  set; }
    public MapObjectShape Shape => _shape;
    public TowerType TowerType => _towerType;
    public bool IsActive { get; set; }

    public int GetSpecPrice(int index) => 0;
    public void SetUniqueTowerIndex(int index) { }
    public void Dispose() { }

    public void OnPreload()
    {
        IsEnabled = false;

        _targetingModule = GetComponent<TargetingModule>();
        _targetingModule.SetTargetSortingTypes(TypeTargetByCharacteristic.Speed, TypeTargetByDistance.ToExit);

        _upgradeController = new TowerUpgradeController(this);
        _upgradeController.Purchase(UpgradeTree);
    }

    public void OnActivated()
    {
    }

    public void OnDeactivated()
    {
        ClearModules();
    }

    public void OnReturned() { }
    public void OnDestroyed() { }
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