using System.Collections.Generic;
using UnityEngine;



public class Rail : AtackTower
{
    [Header("Projectiles")]
    [SerializeField] private RailProjectile _defaultProjectile;
    [Space]
    [SerializeField] private Transform _spawnpoint;
    [SerializeField] private Animator _animator;
    [Space]
    [Header("Critical | first spec")]
    [SerializeField, Range(0, 1)] private float _l3CriticalChance;
    [SerializeField, Min(1)] private float _l3CriticalDamageMultiplier;
    [Space]
    [SerializeField, Range(0, 1)] private float _l4CriticalChance;
    [SerializeField, Min(1)] private float _l4CriticalDamageMultiplier;
    [Space]
    [Header("Break armor | second spec")]
    [SerializeField, Range(0, 1)] private float _l3DecreaseArmor;
    [Space]
    [SerializeField, Range(0, 1)] private float _l4DecreaseArmor;
    [Space]

    private SpecTypeRail _specType;
    private RailProjectile _currProjectile;

    private float _currCriticalChance;
    private float _currCriticalDamageMultiplier;

    private float _currDecreaseArmor;

    void Start()
    {
        _currProjectile = _defaultProjectile;

        //if (_currLevel == 0)
        //{
        //    Upgrade();
        //}
    }

    private void Atack()
    {
        Shoot();

        _animator.SetTrigger("_atack");

        _canAtack = false;
        RailProjectile projectile = Instantiate(_currProjectile, _spawnpoint.position, Quaternion.identity);
        Destroy(projectile.gameObject, 5f);
        projectile.SetDamage(_currDamage);
        projectile.SetArmorPiercing(_currArmorPiercing);

        //if (_hasFirstSpec)
        //{
        //    projectile.SetRailProjectileType(RailProjetileType.Crit);
        //}
        //else if (_hasSecondSpec)
        //{
        //    projectile.SetRailProjectileType(RailProjetileType.ArmorBreak);
        //}

        if (_specType == SpecTypeRail.Critical)
        {
            projectile.SetCritChance(_currCriticalChance);
            projectile.SetCritMultiplier(_currCriticalDamageMultiplier);
        }
        else if (_specType == SpecTypeRail.BreakArmor)
        {
            projectile.SetArmorBreak(_currDecreaseArmor);
        }

        if (_targetEnemy == null)
        {
            projectile.DestroyProjectile();
        }
        else
        {
            projectile._target = _targetEnemy;
            //StartCoroutine(MoveProjectile(projectile));
        }
    }

    public bool Upgrade()
    {
        if (UpgradeAtackTower())
        {
            //if (_hasFirstSpec)
            //{
            //    if (_currLevel == 3)
            //    {
            //        _currCriticalChance = _l3CriticalChance;
            //        _currCriticalDamageMultiplier = _l3CriticalDamageMultiplier;
            //    }
            //    else if (_currLevel == 4)
            //    {
            //        _currCriticalChance = _l4CriticalChance;
            //        _currCriticalDamageMultiplier = _l4CriticalDamageMultiplier;
            //    }
            //}
            //else if (_hasSecondSpec)
            //{
            //    if (_currLevel == 3)
            //    {
            //        _currDecreaseArmor = _l3DecreaseArmor;
            //    }
            //    else if (_currLevel == 4)
            //    {
            //        _currDecreaseArmor = _l4DecreaseArmor;
            //    }
            //}

            //_animator.SetInteger("_currTowerLevel", _currLevel);
            //_animator.SetBool("_hasFirstSpec", _hasFirstSpec);
            //_animator.SetBool("_hasSecondSpec", _hasSecondSpec);

            return true;
        }

        return false;
    }

    public void SetSpecType(int index)
    {
        _specType = (SpecTypeRail)index;

        //if (index == 1 && !_hasSecondSpec)
        //{
        //    _hasFirstSpec = true;
        //    Upgrade();
        //}
        //else if (index == 2 && !_hasFirstSpec)
        //{
        //    _hasSecondSpec = true;
        //    Upgrade();
        //}
    }
}
