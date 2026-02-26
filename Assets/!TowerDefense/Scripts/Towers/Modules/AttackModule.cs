using System.Collections.Generic;
using UnityEngine;

public sealed class AttackModule : ITowerModule
{
    private AttackModuleConfig _config;
    private readonly Tower _tower;
    private float _cooldown;
    private Enemy _target;

    private readonly HashSet<IOnHitEffect> _onHitEffects = new();

    public ModuleType ModuleType => ModuleType.Attack;

    public AttackModule(AttackModuleConfig config, Tower tower)
    {
        _config = config;
        _tower = tower;
    }

    public bool TryApplyConfig(TowerModuleConfig config)
    {
        if (config is not AttackModuleConfig attackConfig)
            return false;

        _config = attackConfig;

        return true;
    }

    public void RegisterOnHitEffect(IOnHitEffect effect)
    {
        _onHitEffects.Add(effect);
    }

    public void Tick(float dt)
    {
        UpdateTarget();
        RotateTower(dt);

        if (_target == null)
            return;

        if (_cooldown > 0)
        {
            _cooldown -= dt;
            return;
        }

        Fire();

        _cooldown = 1f / _config.FireRate;
    }

    private void Fire()
    {
        //Debug.Log($"" +
        //    $"Damage [{_config.Damage}]\n" +
        //    $"Piercing [{_config.Piercing}]\n" +
        //    $"Target [{_target.name}]");

        _target.TakeDamage(_config.Damage, _config.Piercing);
        _tower.TowerRecoil.PlayRecoil();
        _tower.PlayParticle();

        var hit = new HitContext
        {
            Enemy = _target,
            Damage = _config.Damage,
            HitPoint = _target.transform.position
        };

        foreach (var effect in _onHitEffects)
            effect.OnHit(hit);
    }

    private void RotateTower(float dt)
    {
        if (_tower.TargetingModule.IsValid(_target))
        {
            _tower.transform.RotateAt2D(_target.transform.position, _config.RotationSpeed);
        }
    }

    private void UpdateTarget()
    {
        if (_tower.TargetingModule.IsValid(_target))
            return;

        _target = _tower.TargetingModule.GetTarget();
    }
}
