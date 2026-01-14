using System.Collections.Generic;
using UnityEngine;

public class Gravity : Tower
{
    [Header("Levels")]
    [Header("Level 1")]
    [SerializeField, Range(0, 1)] private float _l1SpeedDivisor;
    [Space]
    [Header("Level 2")]
    [SerializeField, Range(0, 1)] private float _l2SpeedDivisor;
    [Space]
    [Header("Level 3")]
    [SerializeField, Range(0, 1)] private float _l3SpeedDivisor;
    [Space]
    [Header("Level 4")]
    [SerializeField, Range(0, 1)] private float _l4SpeedDivisor;
    [Space]
    [Space]
    [Header("Additional money | first spec")]
    [SerializeField, Min(0)] private float _l3MoneyDropMultuplier;
    [Space]
    [SerializeField, Min(0)] private float _l4MoneyDropMultuplier;
    [Space]
    [Space]
    [Header("Heal point division | second spec")]
    [SerializeField, Min(0)] private float _l3HealPointDivisor;
    [Space]
    [SerializeField, Min(0)] private float _l4HealPointDivisor;
    [Space]
    [Space]
    [SerializeField] private Animator _animator;
    [SerializeField, Min(0)] private float _range;

    private SpecTypeGravity _specType;
    private float _currSpeedDivisor;
    private float _currRange;

    private float _currMoneyDropMiltiplier;

    private float _currHealPointDivisor;

    public float CurrRange => _range;
    public SpecTypeGravity SpecTypeGravity => _specType;
    public float CurrSpeedDivisor => _currSpeedDivisor;
    public float CurrMoneyDropMultiplier => _currMoneyDropMiltiplier;
    public float CurrHealPointDivisor => _currHealPointDivisor;

    private void Start()
    {
        _currRange = _range;

        //if (_currLevel == 0)
        //{
        //    Upgrade();
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            //enemy.EnterGravity(_uniqueTowerIndex, this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            //collision.GetComponent<Enemy>().ExitGravity(_uniqueTowerIndex);
        }
    }

    public bool Upgrade()
    {
        //if (UpgradeTower())
        //{
        //    if (_currLevel == 1)
        //    {
        //        _currSpeedDivisor = _l1SpeedDivisor;
        //    }
        //    else if (_currLevel == 2)
        //    {
        //        _currSpeedDivisor = _l2SpeedDivisor;
        //    }
        //    else if (_currLevel == 3)
        //    {
        //        _currSpeedDivisor = _l3SpeedDivisor;
        //    }
        //    else if (_currLevel == 4)
        //    {
        //        _currSpeedDivisor = _l4SpeedDivisor;
        //    }

        //    if (_hasFirstSpec)
        //    {
        //        if (_currLevel == 3)
        //        {
        //            _currMoneyDropMiltiplier = _l3MoneyDropMultuplier;
        //        }
        //        else if (_currLevel == 4)
        //        {
        //            _currMoneyDropMiltiplier = _l4MoneyDropMultuplier;
        //        }
        //    }
        //    else if (_hasSecondSpec)
        //    {
        //        if (_currLevel == 3)
        //        {
        //            _currHealPointDivisor = _l3HealPointDivisor;
        //        }
        //        else if (_currLevel == 4)
        //        {
        //            _currHealPointDivisor = _l4HealPointDivisor;
        //        }
        //    }

        //    _animator.SetInteger("_currTowerLevel", _currLevel);
        //    _animator.SetBool("_hasFirstSpec", _hasFirstSpec);
        //    _animator.SetBool("_hasSecondSpec", _hasSecondSpec);

        //    return true;
        //}

        return false;
    }

    public void SetSpecType(int index)
    {
        _specType = (SpecTypeGravity)index;

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
