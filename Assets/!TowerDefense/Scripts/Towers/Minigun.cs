using UnityEngine;

public class Minigun : AtackTower
{
    [Header("Projectiles")]
    [SerializeField] private MinigunProjectile _defaultProjectile;
    [SerializeField] private MinigunProjectile _explosionProjectile;
    [SerializeField] private MinigunProjectile _freezeProjectile;
    [Space]
    [Header("Explosion | first spec")]
    [SerializeField] private int _l3ExplosionDamage;
    [SerializeField] private float _l3ExplosionRadius;
    [Space]
    [SerializeField] private int _l4ExplosionDamage;
    [SerializeField] private float _l4ExplosionRadius;
    [Space]
    [Header("Freeze | second spec")]
    [SerializeField] private float _l3FreezeRadius;
    [SerializeField] private int _l3FreezeIncrement;
    [Space]
    [SerializeField] private float _l4FreezeRadius;
    [SerializeField] private int _l4FreezeIncrement;
    [Space]
    [SerializeField] private Transform _spawnpoint;
    [SerializeField] private Animator _animator;

    private SpecTypeMinigun _specType;
    private MinigunProjectile _currProjectile;

    private int _currExplosionDamage;
    private float _currExplosionRadius;

    private float _currFreezeRadius;
    private int _currFrezzeIncrement;   

    private string _currAnimation;

    public int CurrExplosionDamage => _currExplosionDamage;
    public float CurrExplosionRadius => _currExplosionRadius;

    public float CurrFreezeRadius => _currFreezeRadius;
    public int CurrFrezzeIncrement => _currFrezzeIncrement;

    void Start()
    {
        if (_specType == SpecTypeMinigun.None)
        {
            _currProjectile = _defaultProjectile;
        }
        else if (_specType == SpecTypeMinigun.Explosion)
        {
            _currProjectile = _explosionProjectile;
        }
        else if (_specType == SpecTypeMinigun.Freeze)
        {
            _currProjectile = _freezeProjectile;
        }

        //if (_currLevel == 0)
        //{
        //    Upgrade();
        //}
    }

    void ChangeAnimation(string nameAnimation)
    {
        if (_currAnimation == nameAnimation)
        {
            return;
        }

        _animator.Play(nameAnimation);
        _currAnimation = nameAnimation;
    }

    void Update()
    {
        if (_atackTimer > 0)
        {
            _atackTimer -= Time.deltaTime;
        }

        if (_targetEnemy == null)
        {
            Enemy enemy = null;

            if (_specType == SpecTypeMinigun.Freeze)
            {
                enemy = GetNearestUnfreezedEnemy();
            }
            else
            {
                enemy = GetNearestEnemy();
            }

            if (enemy != null && Vector2.Distance(transform.position, enemy.transform.position) <= _currMaxAtackRadius)
            {
                _targetEnemy = enemy;
            }

            _animator.SetBool("_isAtack", false);
        }
        else
        {
            _animator.SetBool("_isAtack", true);
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

            if (_specType == SpecTypeMinigun.Freeze)
            {
                _targetEnemy = null;
            }
        }
    }

    private void Atack()
    {
        Shoot();

        _canAtack = false;
        MinigunProjectile projectile = Instantiate(_currProjectile, _spawnpoint.position, Quaternion.identity);
        Destroy(projectile.gameObject, 5f);
        projectile.SetDamage(_currDamage);
        projectile.SetArmorPiercing(_currArmorPiercing);

        if (_specType == SpecTypeMinigun.Explosion)
        {
            projectile.SetExplosionDamage(_currExplosionDamage);
            projectile.SetExplosionRadius(_currExplosionRadius);
        }

        if (_specType == SpecTypeMinigun.Freeze)
        {
            projectile.SetFreezeIncrement(_currFrezzeIncrement);
            projectile.SetFreezeRadius(_currFreezeRadius);
        }

        if (_targetEnemy == null)
        {
            projectile.DestroyProjectile();
        }
        else
        {
            projectile._target = _targetEnemy;
            StartCoroutine(MoveProjectile(projectile));
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
            //        _currExplosionDamage = _l3ExplosionDamage;
            //        _currExplosionRadius = _l3ExplosionRadius;
            //    }
            //    else if (_currLevel == 4)
            //    {
            //        _currExplosionDamage = _l4ExplosionDamage;
            //        _currExplosionRadius = _l4ExplosionRadius;
            //    }
            //}
            //else if (_hasSecondSpec)
            //{
            //    if (_currLevel == 3)
            //    {
            //        _currFreezeRadius = _l3FreezeRadius;
            //        _currFrezzeIncrement = _l3FreezeIncrement;
            //    }
            //    else if (_currLevel == 4)
            //    {
            //        _currFreezeRadius = _l4FreezeRadius;
            //        _currFrezzeIncrement = _l4FreezeIncrement;
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
        _specType = (SpecTypeMinigun)index;

        //if (index == 1 && !_hasSecondSpec)
        //{
        //    _hasFirstSpec = true;

        //    _currProjectile = _explosionProjectile;

        //    Upgrade();
        //}
        //else if (index == 2 && !_hasFirstSpec)
        //{
        //    _hasSecondSpec = true;

        //    _currProjectile = _freezeProjectile;

        //    Upgrade();
        //}
    }
}

