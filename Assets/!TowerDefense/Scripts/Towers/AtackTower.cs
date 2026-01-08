using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackTower : Tower
{
    [Header("Atack")]
    [SerializeField] protected float _speedProjectile;
    [Space]
    [Header("Level 1")]
    [SerializeField, Min(0)] protected int _l1Damage;
    [SerializeField, Range(0, 1)] protected float _l1ArmorPiercing;
    [SerializeField, Min(0)] protected float _l1DelayBtwAtack;
    [SerializeField, Min(0)] protected float _l1MinAtackRadius;
    [SerializeField, Min(0)] protected float _l1MaxAtackRadius;
    [Space]
    [SerializeField] private float l1TESTAtackPerSecond;
    [SerializeField] private float l1TESTDamagePerSeconds;
    [Space]
    [Header("Level 2")]
    [SerializeField, Min(0)] protected int _l2Damage;
    [SerializeField, Range(0, 1)] protected float _l2ArmorPiercing;
    [SerializeField, Min(0)] protected float _l2DelayBtwAtack;
    [SerializeField, Min(0)] protected float _l2MinAtackRadius;
    [SerializeField, Min(0)] protected float _l2MaxAtackRadius;
    [Space]
    [SerializeField] private float l2TESTAtackPerSecond;
    [SerializeField] private float l2TESTDamagePerSeconds;
    [Space]
    [Header("Level 3")]
    [SerializeField, Min(0)] protected int _l3Damage;
    [SerializeField, Range(0, 1)] protected float _l3ArmorPiercing;
    [SerializeField, Min(0)] protected float _l3DelayBtwAtack;
    [SerializeField, Min(0)] protected float _l3MinAtackRadius;
    [SerializeField, Min(0)] protected float _l3MaxAtackRadius;
    [Space]
    [SerializeField] private float l3TESTAtackPerSecond;
    [SerializeField] private float l3TESTDamagePerSeconds;
    [Space]
    [Header("Level 4")]
    [SerializeField, Min(0)] protected int _l4Damage;
    [SerializeField, Range(0, 1)] protected float _l4ArmorPiercing;
    [SerializeField, Min(0)] protected float _l4DelayBtwAtack;
    [SerializeField, Min(0)] protected float _l4MinAtackRadius;
    [SerializeField, Min(0)] protected float _l4MaxAtackRadius;
    [Space]
    [SerializeField] private float l4TESTAtackPerSecond;
    [SerializeField] private float l4TESTDamagePerSeconds;
    [Space]
    [Header("Rotation")]
    [SerializeField] protected float _speedRotationTower;
    [Space]
    [SerializeField] private AudioSource _soundShoot;
    [Space]
    [Space]

    protected int _currDamage;
    protected float _currArmorPiercing;
    protected float _currDelayBtwAtack;
    protected float _currMinAtackRadius;
    protected float _currMaxAtackRadius;
    protected float _atackTimer;
    protected bool _canAtack;
    protected Enemy _targetEnemy;

    private Vector3 _dir;
    private float _angleDirection;
    private Quaternion _targetRotation;

    public int L1Damage => _l1Damage;
    public float L1ArmorPiercing => _l1ArmorPiercing;
    public float L1DelayBtwAtack => _l1DelayBtwAtack;
    public float L1MinAtackRadius => _l1MinAtackRadius;
    public float L1MaxAtackRadius => _l1MaxAtackRadius;

    public int L2Damage => _l2Damage;
    public float L2ArmorPiercing => _l2ArmorPiercing;
    public float L2DelayBtwAtack => _l2DelayBtwAtack;
    public float L2MinAtackRadius => _l2MinAtackRadius;
    public float L2MaxAtackRadius => _l2MaxAtackRadius;

    public int L3Damage => _l3Damage;
    public float L3ArmorPiercing => _l3ArmorPiercing;
    public float L3DelayBtwAtack => _l3DelayBtwAtack;
    public float L3MinAtackRadius => _l3MinAtackRadius;
    public float L3MaxAtackRadius => _l3MaxAtackRadius;

    public int L4Damage => _l4Damage;
    public float L4ArmorPiercing => _l4ArmorPiercing;
    public float L4DelayBtwAtack => _l4DelayBtwAtack;
    public float L4MinAtackRadius => _l4MinAtackRadius;
    public float L4MaxAtackRadius => _l4MaxAtackRadius;

    public int CurrDamage => _currDamage;
    public float CurrArmorPiercing => _currArmorPiercing;
    public float CurrDelayBtwAtack => _currDelayBtwAtack;
    public float CurrMinAtackRadius => _currMinAtackRadius;
    public float CurrMaxAtackRadius => _currMaxAtackRadius;

    private void OnValidate()
    {
        l1TESTAtackPerSecond = 1 / _l1DelayBtwAtack;
        l2TESTAtackPerSecond = 1 / _l2DelayBtwAtack;
        l3TESTAtackPerSecond = 1 / _l3DelayBtwAtack;
        l4TESTAtackPerSecond = 1 / _l4DelayBtwAtack;

        l1TESTDamagePerSeconds = l1TESTAtackPerSecond * _l1Damage;
        l2TESTDamagePerSeconds = l2TESTAtackPerSecond * _l2Damage;
        l3TESTDamagePerSeconds = l3TESTAtackPerSecond * _l3Damage;
        l4TESTDamagePerSeconds = l4TESTAtackPerSecond * _l4Damage;

        _currMinAtackRadius = _l1MinAtackRadius;
        _currMaxAtackRadius = _l1MaxAtackRadius;
    }

    protected void RotateTower()
    {
        if (_targetEnemy != null)
        {
            _dir = _targetEnemy.transform.position - transform.position;
            _angleDirection = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
            _targetRotation = Quaternion.AngleAxis(_angleDirection - 90, Vector3.forward);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _speedRotationTower * Time.deltaTime);
        }
    }

    protected IEnumerator MoveProjectile(Projectile projectile)
    {
        while (GetTargetDistance(projectile!._target) > 0.2f && projectile != null && projectile!._target != null)
        {
            var dir = projectile!._target.transform.position - transform.position;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection - 90, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.position, projectile!._target.transform.position, _speedProjectile * Time.deltaTime);

            yield return null;
        }

        if (projectile != null && projectile!._target == null)
        {
            projectile.DestroyProjectile();
        }
    }

    protected float GetTargetDistance(Enemy enemy)
    {
        if (enemy == null)
        {
            enemy = GetNearestEnemy();

            if (enemy == null)
            {
                return 0f;
            }
        }

        return Mathf.Abs(Vector2.Distance(transform.position, enemy.transform.position));
    }

    protected List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach (var enemy in WaveController.Instance.Enemies)
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) < _currMaxAtackRadius)
            {
                if (Vector2.Distance(enemy.transform.position, transform.position) > _currMinAtackRadius)
                {
                    enemiesInRange.Add(enemy);
                }
            }
        }

        return enemiesInRange;
    }

    protected Enemy GetRandomEnemyInRange()
    {
        List<Enemy> enemiesInRange = GetEnemiesInRange();
        if (enemiesInRange.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, enemiesInRange.Count);
        return enemiesInRange[randomIndex];
    }

    protected Enemy GetNearestUnfreezedEnemy()
    {
        List<Enemy> enemiesInRange = GetEnemiesInRange();

        if (enemiesInRange.Count == 0)
        {
            return null;
        }

        List<Enemy> unfreezedEnemies = new List<Enemy>();

        foreach (var item in enemiesInRange)
        {
            if (!item.MaxFreeze)
            {
                unfreezedEnemies.Add(item);
            }
        }

        if (unfreezedEnemies.Count == 0)
        {
            return GetNearestEnemy();
        }

        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;

        foreach (Enemy enemy in unfreezedEnemies)
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.position, enemy.transform.position);
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    protected Enemy GetNearestEnemy()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;

        foreach (Enemy enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.position, enemy.transform.position);
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    protected bool UpgradeAtackTower()
    {
        if (UpgradeTower())
        {
            if (_currLevel == 1)
            {
                _currDamage = _l1Damage;
                _currArmorPiercing = _l1ArmorPiercing;
                _currDelayBtwAtack = _l1DelayBtwAtack;
                _currMinAtackRadius = _l1MinAtackRadius;
                _currMaxAtackRadius = _l1MaxAtackRadius;
            }
            else if (_currLevel == 2)
            {
                _currDamage = _l2Damage;
                _currArmorPiercing = _l2ArmorPiercing;
                _currDelayBtwAtack = _l2DelayBtwAtack;
                _currMinAtackRadius = _l2MinAtackRadius;
                _currMaxAtackRadius = _l2MaxAtackRadius;
            }
            else if (_currLevel == 3)
            {
                _currDamage = _l3Damage;
                _currArmorPiercing = _l3ArmorPiercing;
                _currDelayBtwAtack = _l3DelayBtwAtack;
                _currMinAtackRadius = _l3MinAtackRadius;
                _currMaxAtackRadius = _l3MaxAtackRadius;
            }
            else if (_currLevel == 4)
            {
                _currDamage = _l4Damage;
                _currArmorPiercing = _l4ArmorPiercing;
                _currDelayBtwAtack = _l4DelayBtwAtack;
                _currMinAtackRadius = _l4MinAtackRadius;
                _currMaxAtackRadius = _l4MaxAtackRadius;
            }

            return true;
        }

        return false;
    }

    protected void Shoot()
    {
        _soundShoot.pitch = Random.Range(0.9f, 1.1f);
        _soundShoot.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _currMinAtackRadius);
        Gizmos.DrawWireSphere(transform.position, _currMaxAtackRadius);
    }
}
