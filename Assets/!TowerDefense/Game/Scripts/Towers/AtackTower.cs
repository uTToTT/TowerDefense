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

    public int CurrDamage => _currDamage;
    public float CurrArmorPiercing => _currArmorPiercing;
    public float CurrDelayBtwAtack => _currDelayBtwAtack;
    public float CurrMinAtackRadius => _currMinAtackRadius;
    public float CurrMaxAtackRadius => _currMaxAtackRadius;

    protected bool UpgradeAtackTower()
    {
        return false;
    }


    protected void Shoot()
    {
        _soundShoot.pitch = Random.Range(0.9f, 1.1f);
        _soundShoot.Play();
    }
}
