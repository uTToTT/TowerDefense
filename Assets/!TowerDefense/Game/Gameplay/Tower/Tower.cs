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

    public bool IsEnabled=> _enabled; 
    public Transform Transform => transform;
    public TowerType TowerType => _towerType;

    public void Enable() => _enabled = true;
    public void Disable() => _enabled = false;

    public void ShowRange() => _towerPreview.Enable();
    public void HideRange() => _towerPreview.Disable();

    public void PlayParticle() => _particles?.Play();

    #region Init

    private void Awake()
    {
        _targetingModule = GetComponent<TargetingModule>();
        _targetingModule.SetTargetSortingTypes(TypeTargetByCharacteristic.Speed, TypeTargetByDistance.ToExit);

        _upgradeController = new TowerUpgradeController(this);
    }

    #endregion

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
        }

        if (modulesChanged)
        {
            BindOnHitEffects();
        }
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

    #region Game loop

    public void Tick(float dt)
    {
        if (!IsEnabled)
            return;

        _targetingModule.Tick(dt);

        foreach (var module in _modules.Values)
            module.Tick(dt);

    }

    #endregion

    #region Modules

    private void AddModule(ITowerModule module)
    {
        if (_modules.ContainsKey(module.ModuleType))
        {
#if UNITY_EDITOR
            Debug.Log($"Module already exists [{module.GetType()}]");
#endif
        }
        else
        {
            _modules.Add(module.ModuleType, module);
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

    private void ClearModules()
    {
        _modules.Clear();
    }

    #endregion

    #region Lifecycle

    public override void OnPreload()
    {
        base.OnPreload();

        _enabled = false;

      
    }

    public override void OnActivated()
    {
        base.OnActivated();
        ClearModules();
        _upgradeController.Restart();
        _upgradeController.Purchase(UpgradeTree);
        Enable();
    }

    public override void OnDeactivated()
    {
        base.OnDeactivated();
        Disable();
    }

    public override void OnReturned()
    {
        base.OnReturned();
        transform.rotation = new Quaternion();
        _targetingModule.Restart();
        Disable();
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public override void OnDestroyed()
    {
        base.OnDestroyed();
    }

    #endregion
}

