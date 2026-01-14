using UnityEngine;

public sealed class AttackModule : ITowerModule
{
    private readonly AttackModuleConfig _config;
    private readonly Tower _tower;
    private float _cooldown;
    private Enemy _target;

    public AttackModule(AttackModuleConfig config, Tower tower)
    {
        _config = config;
        _tower = tower;
    }

    public bool TryApplyConfig(TowerModuleConfig config)
    {
        if (config is not AttackModuleConfig)
            return false;

        return true;
    }

    public void Tick(float dt)
    {
        RotateTower(dt);

        if (_cooldown > 0)
        {
            _cooldown -= dt;
            return;
        }

        //TargetingService.FindTarget(
        //    _tower.transform.position,
        //    _config.MinRange
        //);

        if (_target == null) return;

        _target.TakeDamage(_config.Damage, _config.Piercing);
        _cooldown = 1f / _config.FireRate;
    }

    protected void RotateTower(float dt)
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
