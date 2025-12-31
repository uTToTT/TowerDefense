using UnityEngine;

public class Twiin : AtackTower
{
    [Header("Projectiles")]
    [SerializeField] private TwiinProjectile _defaultProjectile;
    [Space]
    [Header("Two one | first spec")]
    [Header("Level 3")]
    [SerializeField] private int _l3DamageTwoOne;
    [Space]
    [Header("Level 4")]
    [SerializeField] private int _l4DamageTwoOne;
    [Space]
    [Header("Shard explode | first spec")]
    [Header("Level 3")]
    [SerializeField] private float _l3DistanceShardExplode;
    [SerializeField] private int _l3ShardDamage;
    [SerializeField] private int _l3MaxNumShard;
    [SerializeField] private float _l3ShardSpeed;
    [Space]
    [Header("Level 4")]
    [SerializeField] private float _l4DistanceShardExplode;
    [SerializeField] private int _l4ShardDamage;
    [SerializeField] private int _l4NumShard;
    [SerializeField] private float _l4ShardSpeed;
    [Space]
    [Space]
    [SerializeField] private Transform[] _spawnpoints;
    [SerializeField] private Animator _animator;

    private SpecTypeTwiin _specType;
    private TwiinProjectile _currProjectile;
    private int _bulletCounter;

    private int _currDamageTwoOne;

    private float _currDistanceShardExplode;
    private int _currShardDamage;
    private int _currNumShard;
    private float _currShardSpeed;

    public int CurrNumShard => _currNumShard;
    public float CurrentDamageTwoOne => _currDamageTwoOne;
    public float CurrShardDamage => _currShardDamage;
    public SpecTypeTwiin SpecTypeTwiin => _specType;

    void Start()
    {
        _currProjectile = _defaultProjectile;

        if (_currLevel == 0)
        {
            Upgrade();
        }
    }

    void Update()
    {
        if (_atackTimer > 0)
        {
            _atackTimer -= Time.deltaTime;
        }

        if (_targetEnemy == null)
        {
            Enemy nearestEnemy = GetNearestEnemy();

            if (nearestEnemy != null && Vector2.Distance(transform.position, nearestEnemy.transform.position) <= _currMaxAtackRadius)
            {
                _targetEnemy = nearestEnemy;
            }
        }
        else
        {
            RotateTower();

            if (_atackTimer <= 0)
            {
                _canAtack = true;

                _atackTimer = _currDelayBtwAtack;
            }
            else
            {
                _canAtack = false;
            }

            if (Vector2.Distance(transform.position, _targetEnemy.transform.position) > _currMaxAtackRadius)
            {
                _targetEnemy = null;
            }
            else if (Vector2.Distance(transform.position, _targetEnemy.transform.position) < _currMinAtackRadius)
            {
                _targetEnemy = null;
            }
        }

        if (_canAtack)
        {
            Atack();
        }
    }

    private void Atack()
    {
        Shoot();

        _canAtack = false;
        //Debug.Log("Curr num shard" + _currNumShard);

        TwiinProjectile projectile_0 = Instantiate(_currProjectile, _spawnpoints[_bulletCounter % _spawnpoints.Length].position, Quaternion.identity);
        _bulletCounter++;
        Destroy(projectile_0.gameObject, 5f);
        projectile_0.SetDamage(_currDamage);
        projectile_0.SetArmorPiercing(_currArmorPiercing);

        if (_specType == SpecTypeTwiin.Shard)
        {
            projectile_0.SetTwiinProjectileType(TwiinProjectileType.Shard);
            projectile_0.SetShardDamage(_currShardDamage);
            projectile_0.SetMaxNumShard(_currNumShard);
            projectile_0.SetShardExplodeRadius(_currDistanceShardExplode);
            projectile_0.SetShardSpeed(_currShardSpeed);
        }

        if (_specType != SpecTypeTwiin.TwoToOneAtack)
        {
            if (_bulletCounter % _spawnpoints.Length == 0)
            {
                _animator.SetTrigger("_rightAtack");
            }
            else if (_bulletCounter % _spawnpoints.Length == 1)
            {
                _animator.SetTrigger("_leftAtack");
            }
        }
        else if (_specType == SpecTypeTwiin.TwoToOneAtack)
        {
            _animator.SetTrigger("_atack");
        }

        if (_targetEnemy == null)
        {
            projectile_0.DestroyProjectile();
        }
        else
        {
            projectile_0._target = _targetEnemy;
            StartCoroutine(MoveProjectile(projectile_0));
        }

        if (_specType == SpecTypeTwiin.TwoToOneAtack)
        {
            TwiinProjectile projectile_1 = Instantiate(_currProjectile, _spawnpoints[_bulletCounter % _spawnpoints.Length].position, Quaternion.identity);
            _bulletCounter++;
            Destroy(projectile_1.gameObject, 5f);
            if (_targetEnemy == null)
            {
                projectile_1.DestroyProjectile();
            }
            else
            {
                projectile_1._target = _targetEnemy;
                StartCoroutine(MoveProjectile(projectile_1));
            }

            projectile_0.SetDamage(_currDamageTwoOne);
            projectile_1.SetDamage(_currDamageTwoOne);
            projectile_1.SetArmorPiercing(_currArmorPiercing);
        }
    }

    public bool Upgrade()
    {
        if (UpgradeAtackTower())
        {
            if (_hasFirstSpec)
            {
                if (_currLevel == 3)
                {
                    _currDamageTwoOne = _l3DamageTwoOne;
                }
                else if (_currLevel == 4)
                {
                    _currDamageTwoOne = _l4DamageTwoOne;
                }
            }
            else if (_hasSecondSpec)
            {
                if (_currLevel == 3)
                {
                    _currDistanceShardExplode = _l3DistanceShardExplode;
                    _currShardDamage = _l3ShardDamage;
                    _currNumShard = _l3MaxNumShard;
                    _currShardSpeed = _l3ShardSpeed;
                }
                else if (_currLevel == 4)
                {
                    _currDistanceShardExplode = _l4DistanceShardExplode;
                    _currShardDamage = _l4ShardDamage;
                    _currNumShard = _l4NumShard;
                    _currShardSpeed = _l4ShardSpeed;
                }
            }

            _animator.SetInteger("_currTowerLevel", _currLevel);
            _animator.SetBool("_hasFirstSpec", _hasFirstSpec);
            _animator.SetBool("_hasSecondSpec", _hasSecondSpec);
            
            return true;
        }

        return false;
    }

    public void SetSpecType(int index)
    {
        _specType = (SpecTypeTwiin)index;

        if (index == 1 && !_hasSecondSpec)
        {
            _hasFirstSpec = true;
            Upgrade();
        }
        else if (index == 2 && !_hasFirstSpec)
        {
            _hasSecondSpec = true;
            Upgrade();
        }
    }
}
