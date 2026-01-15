using System.Collections.Generic;
using UnityEngine;

public sealed class AttackModule : ITowerModule
{
    private AttackModuleConfig _config;
    private readonly Tower _tower;
    private float _cooldown;
    private Enemy _target;

    private readonly HashSet<IOnHitEffect> _onHitEffects = new();

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
        RotateTower(dt);

        if (_cooldown > 0)
        {
            _cooldown -= dt;
            return;
        }

        _target = _tower.TargetingModule.GetTarget();

        if (_target == null) return;

        _target.TakeDamage(_config.Damage, _config.Piercing);

        var hit = new HitContext
        {
            Enemy = _target,
            Damage = _config.Damage,
            HitPoint = _target.transform.position
        };

        foreach (var effect in _onHitEffects)
            effect.OnHit(hit);

        _cooldown = 1f / _config.FireRate;
    }

    private void RotateTower(float dt)
    {
        if (_target != null)
        {
            var dir = _target.transform.position - _tower.transform.position;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            var targetRotation =
                Quaternion.AngleAxis(
                    angleDirection - 90,
                    Vector3.forward);

            _tower.transform.rotation =
                Quaternion.RotateTowards(
                    _tower.transform.rotation,
                    targetRotation,
                    _config.RotationSpeed * dt);
        }
    }
}
