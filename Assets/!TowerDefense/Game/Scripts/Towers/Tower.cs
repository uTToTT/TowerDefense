using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TargetingModule))]
public class Tower : MapObject, IPoolable, IEntityLifecycle
{
    [SerializeField] protected TowerType _towerType;
    [SerializeField] private bool _enabled;
    [HorizontalLine]

    [SerializeField] private UpgradeNodeConfig _upgradeTree;
    [SerializeField] private TowerPreview _towerPreview;
    [SerializeField] private CustomParticleSystem _particles;
    [SerializeField] private TowerRecoil _towerRecoil;
    [SerializeField] private Transform _towerTransform;

    private TowerUpgradeController _upgradeController;
    private TargetingModule _targetingModule;
    private readonly Dictionary<ModuleType, ITowerModule> _modules = new();

    public UpgradeNodeConfig UpgradeTree => _upgradeTree;
    public TowerUpgradeController UpgradeController => _upgradeController;
    public TargetingModule TargetingModule => _targetingModule;
    public TowerRecoil TowerRecoil => _towerRecoil;
    public Transform TowerTransform => _towerTransform;

    public bool IsEnabled { get => _enabled; private set => _enabled = value; }

    public void Enable() => IsEnabled = true;
    public void Disable() => IsEnabled = false;

    public void ShowRange() => _towerPreview.Enable();
    public void HideRange() => _towerPreview.Disable();

    public void PlayParticle() => _particles?.Play();

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
            Debug.Log($"Apply {moduleConfig.GetType()}");
        }

        if (modulesChanged)
        {
            BindOnHitEffects();
        }
    }

    public bool HasModule(ModuleType moduleType)
    {
        return _modules.ContainsKey(moduleType);
    }

    public T GetModule<T>(ModuleType moduleType) where T : class, ITowerModule
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
            Debug.Log($"Add {module.ModuleType}");
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

        _targetingModule.Tick(dt);

        foreach (var module in _modules.Values)
            module.Tick(dt);

    }

    public Transform Transform => transform;
    public TowerType TowerType => _towerType;

    public int GetSpecPrice(int index) => 0;
    public void SetUniqueTowerIndex(int index) { }
    public override void Dispose()
    {
        base.Dispose();
    }

    public override void OnPreload()
    {
        base.OnPreload();

        IsEnabled = false;

        _targetingModule = GetComponent<TargetingModule>();
        _targetingModule.SetTargetSortingTypes(TypeTargetByCharacteristic.Speed, TypeTargetByDistance.ToExit);

        _upgradeController = new TowerUpgradeController(this);
        _upgradeController.Purchase(UpgradeTree);
    }

    public override void OnActivated()
    {
        base.OnActivated();
    }

    public override void OnDeactivated()
    {
        base.OnDeactivated();
    }

    public override void OnReturned()
    {
        base.OnReturned();
        transform.rotation = new Quaternion();
        ClearModules();
        _targetingModule.Restart();
        _upgradeController.Restart();
        Disable();
    }

    public override void OnDestroyed()
    {
        base.OnDestroyed();
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