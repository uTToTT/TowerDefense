using System.Collections.Generic;
using TToTT.TowerDefense.Enemies;
using UnityEngine;

public sealed class AttackModule : ITowerModule
{
    private AttackModuleConfig _config;
    private readonly Tower _tower;
    private float _cooldown;
    private Enemy _target;

    private readonly HashSet<IOnHitEffect> _onHitEffects = new();

    public ModuleType ModuleType => ModuleType.Attack;

    public AttackModule(
        AttackModuleConfig config,
        Tower tower)
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

        if (!IsAimedAtTarget()) 
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

        Vector2 hitDir = (_target.transform.position - _tower.transform.position).normalized;

        _target.TakeDamage(_config.Damage, _config.Piercing, hitDir);
        _tower.TowerRecoil.PlayRecoil();

        var request = ParticleRequest.At(_config.FireParticle, _tower.FirePoint.position);

        _tower.ParticlesGenerator.Play(request);
        _tower.AudioService.Play(_config.FireSound);

        var hit = new HitContext
        {
            Enemy = _target,
            Damage = _config.Damage,
            HitPoint = _target.transform.position,
            HitDirection = hitDir,
        };

        foreach (var effect in _onHitEffects)
            effect.OnHit(hit);
    }

    private void RotateTower(float dt)
    {
        if (_tower.TargetingModule.IsValid(_target))
        {
            _tower.TowerTransform.RotateAt2D(_target.transform.position, _config.RotationSpeed /** dt*/);
        }
    }

    private void UpdateTarget()
    {
        if (_tower.TargetingModule.IsValid(_target))
            return;

        _target = _tower.TargetingModule.GetTarget();
    }

    private bool IsAimedAtTarget()
    {
        Vector2 toTarget = (_target.transform.position - _tower.TowerTransform.position).normalized;
        Vector2 towerForward = _tower.TowerTransform.up;

        float dot = Vector2.Dot(towerForward, toTarget);
        float threshold = Mathf.Cos(_config.AimThresholdDegrees * Mathf.Deg2Rad);

        return dot >= threshold;
    }
}
