using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TargetingModule))]
public class Tower : MonoBehaviour, IPoolable, IEntityLifecycle, IEnergyNode
{
    [SerializeField] protected TowerType _towerType;
    [SerializeField] private MapObjectShape _shape;
    [HorizontalLine]

    [SerializeField] private UpgradeNodeConfig _upgradeTree;

    private TowerUpgradeController _upgradeController;
    private TargetingModule _targetingModule;
    private readonly Dictionary<ModuleType, ITowerModule> _modules = new();

    public UpgradeNodeConfig UpgradeTree => _upgradeTree;
    public TowerUpgradeController UpgradeController => _upgradeController;
    public TargetingModule TargetingModule => _targetingModule;

    public bool IsEnabled { get; private set; }

    public void Enable() => IsEnabled = true;
    public void Disable() => IsEnabled = false;

    public void ApplyUpgrade(UpgradeNodeConfig config)
    {
        Debug.Log($"Applied upgrade [{config.name}]");
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
        return _modules.ContainsKey(moduleType);
    }

    private T GetModule<T>(ModuleType moduleType) where T : class, ITowerModule
    {
        if (_modules.TryGetValue(moduleType, out var module))
            return module as T;

        return null;
    }

    private void BindOnHitEffects()
    {
        var attack = _modules.OfType<AttackModule>().FirstOrDefault();
        if (attack == null)
            return;

        foreach (var module in _modules.Values)
        {
            if (module is IOnHitEffect effect)
            {
                attack.RegisterOnHitEffect(effect);
            }
        }
    }

    private void AddModule(ITowerModule module)
    {
        if (_modules.ContainsKey(module.ModuleType))
        {
            Debug.Log($"Module already exists [{module.GetType()}]");
        }
        else
        {
            _modules.Add(module.ModuleType, module);
        }
    }

    private void ApplyConfig(TowerModuleConfig config)
    {
        if (config is TargetingModuleConfig)
        {
            _targetingModule.TryApplyConfig(config);
            return;
        }

        if (_modules.ContainsKey(config.ModuleType))
        {
            _modules[config.ModuleType].TryApplyConfig(config);
        }
    }

    private void ClearModules()
    {
        _modules.Clear();
    }

    public void Tick(float dt)
    {
        if (!IsEnabled)
            return;

        foreach (var module in _modules.Values)
            module.Tick(dt);
    }

    public Transform Transform => transform;
    public Vector2Int MapPos { get; set; }
    public MapObjectShape Shape => _shape;
    public TowerType TowerType => _towerType;
    public bool IsActive { get; set; }
    public EnergyNetwork EnergyNetwork { get; set; }

    public float EnergyProduction
    {
        get
        {
            var module = GetModule<EnergyModule>(ModuleType.Energy);
            return module != null ? module.GetProduction() : 0;
        }
    }

    public float EnergyConsumption
    {
        get
        {
            var module = GetModule<EnergyModule>(ModuleType.Energy);
            return module != null ? module.GetConsumption() : 0;
        }
    }

    public float GetEnergyEffectivity()
    {
        var module = GetModule<EnergyModule>(ModuleType.Energy);
        return module != null ? module.GetEnergyEffectivity() : 0;
    }

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

    public void OnNetworkUpdated(EnergyNetwork network)
    {
        //throw new System.NotImplementedException();
    }

    public void SetReceivedEnergy(float amount)
    {
        var module = GetModule<EnergyModule>(ModuleType.Energy);
        module.SetReceivedEnergy(amount);
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