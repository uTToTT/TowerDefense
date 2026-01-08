using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [Header("Test")]
    [Space]
    [SerializeField] private bool _DEBUG;
    [SerializeField, Range(1, 3)] private int _CURRLEVEL;
    [SerializeField] private bool _ISATACK;
    [SerializeField, Min(1)] private float _SPEEDDIVIDE;
    [Space]
    [Header("Damagble tower")]
    [Header("Don't work with Gravity")]
    [SerializeField] private float _minAtackRadius;
    [SerializeField] private float _speedRotation;
    [SerializeField] private float _speedProjectile;
    [Space]
    [SerializeField] private Projectile _projectilePrefab;
    [Space]
    [Header("Properties")]
    [SerializeField] private Transform[] _spawnpoint;
    [SerializeField] private TowerType _type;
    //[SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    [Space]
    [Header("Levels")]
    [Space]
    [Header("Lvl1")]
    [SerializeField] private int _l1Damage;
    [SerializeField] private float _l1MaxAtackRadius;
    [SerializeField] private float _l1DelayBtwAtack;
    [SerializeField] private int _l1SellCost;
    [SerializeField] private int _l1UpgradeCost;
    [Header("(1 - Divisor) * speed")]
    [SerializeField, Range(0, 1f)] private float _l1Divisor;
    [Space]
    [Header("Lvl2")]
    [SerializeField] private int _l2Damage;
    [SerializeField] private float _l2MaxAtackRadius;
    [SerializeField] private float _l2DelayBtwAtack;
    [SerializeField] private int _l2SellCost;
    [SerializeField] private int _l2UpgradeCost;
    [SerializeField] private Sprite _l2Sprite;
    [Header("(1 - Divisor) * speed")]
    [SerializeField, Range(0, 1f)] private float _l2Divisor;
    [Space]
    [Header("Lvl3")]
    [SerializeField] private int _l3Damage;
    [SerializeField] private float _l3MaxAtackRadius;
    [SerializeField] private float _l3DelayBtwAtack;
    [SerializeField] private int _l3SellCost;
    [SerializeField] private int _l3UpgradeCost;
    [SerializeField] private Sprite _l3Sprite;
    [Header("(1 - Divisor) * speed")]
    [SerializeField, Range(0, 1f)] private float _l3Divisor;

    private float _currDelayBtwAtack;
    private float _currMaxAtackRadius;
    private float _currDivisor;
    private int _currSellCost;
    private int _currLvl;
    private int _currDamage;
    private int _currUpgradeCost;

    private float _atackTimer;
    private bool _canAtack;
    private Enemy _targetEnemy = null;
    private int _bulletCounter;

    public TowerType TypeOfTower => _type;
    public int SellCount => _currSellCost;
    public int CurrUpgradeCost => _currUpgradeCost;
    public int CurrLevel => _currLvl;
    public float CurrMaxAtackRadius => _currMaxAtackRadius;

    private void OnValidate()
    {
        if (_type != TowerType.Gravity)
        {
            if (_type != TowerType.Twiin)
            {
                _animator.SetBool("_isAtack", _ISATACK);
            }
        }

        _animator.SetInteger("_currTowerLevel", _CURRLEVEL);

        if (_minAtackRadius >= _currMaxAtackRadius)
        {
            _minAtackRadius = _currMaxAtackRadius - 0.1f;
        }

        _currDelayBtwAtack = _l1DelayBtwAtack;
        _currMaxAtackRadius = _l1MaxAtackRadius;
        _currSellCost = _l1SellCost;
        _currDamage = _l1Damage;
        _currDivisor = _l1Divisor;
        _currUpgradeCost = _l1UpgradeCost;
    }

    private void Start()
    {
        _currLvl = 1;
        _currDelayBtwAtack = _l1DelayBtwAtack;
        _currMaxAtackRadius = _l1MaxAtackRadius;
        _currSellCost = _l1SellCost;
        _currDamage = _l1Damage;
        _currDivisor = _l1Divisor;
        _currUpgradeCost = _l1UpgradeCost;

        if (_type != TowerType.Gravity)
        {
            if (_type != TowerType.Twiin)
            {
                _animator.SetBool("_isAtack", _ISATACK);
            }
        }

        _animator.SetInteger("_currTowerLevel", _CURRLEVEL);
    }

    private void Update()
    {
        if (_DEBUG)
        {

        }
        else
        {
            if (_targetEnemy != null)
            {
                _animator.SetBool("_isAtack", true);
            }
            else
            {
                _animator.SetBool("_isAtack", false);
            }
        }


        if (_type != TowerType.Gravity)
        {
            _atackTimer -= Time.deltaTime;

            if (_DEBUG)
            {
                if (_atackTimer <= 0)
                {
                    _canAtack = true;

                    _atackTimer = _currDelayBtwAtack;
                }
                else
                {
                    _canAtack = false;
                }

                if (_canAtack)
                {
                    Atack();
                }
            }

            if (_targetEnemy == null)
            {
                Enemy nearestEnemy = GetNearestEnemy();

                if (nearestEnemy != null && Vector2.Distance(transform.position, nearestEnemy.transform.position) <= _currMaxAtackRadius)
                {
                    if (Vector2.Distance(transform.position, nearestEnemy.transform.position) >= _minAtackRadius)
                    {
                        _targetEnemy = nearestEnemy;
                    }
                }
            }
            else
            {
                if (_atackTimer <= 0)
                {
                    _canAtack = true;

                    _atackTimer = _currDelayBtwAtack;
                }
                else
                {
                    _canAtack = false;
                }

                var dir = _targetEnemy.transform.position - transform.position;
                var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angleDirection - 90, Vector3.forward);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _speedRotation * Time.deltaTime);

                if (Vector2.Distance(transform.position, _targetEnemy.transform.position) > _currMaxAtackRadius)
                {
                    _targetEnemy = null;
                }
                else if (Vector2.Distance(transform.position, _targetEnemy.transform.position) < _minAtackRadius)
                {
                    _targetEnemy = null;
                }
            }

            if (_canAtack)
            {
                Atack();
            }
        }
    }

    private void Atack()
    {
        _canAtack = false;
        Projectile projectile = Instantiate(_projectilePrefab) as Projectile;
        projectile.SetDamage(_currDamage);
        projectile.transform.position = _spawnpoint[_bulletCounter % _spawnpoint.Length].position;

        if (_type == TowerType.Twiin)
        {
            if (_bulletCounter % _spawnpoint.Length == 0)
            {
                _animator.SetTrigger("_leftAtack");
            }
            else if (_bulletCounter % _spawnpoint.Length == 1)
            {
                _animator.SetTrigger("_rightAtack");
            }
        }
        else
        {
            if (_type != TowerType.Minigun)
            {
                _animator.SetTrigger("_atack");
            }
        }

        _bulletCounter++;

        if (_bulletCounter == _spawnpoint.Length)
        {
            _bulletCounter = 0;
        }
        if (_DEBUG)
        {
            Destroy(projectile.gameObject, 3f);
            StartCoroutine(DebugMoveProjectile(projectile));
        }
        else
        {
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
    }

    private IEnumerator DebugMoveProjectile(Projectile projectile)
    {
        while (projectile != null)
        {
            projectile.transform.Translate(Vector2.up / _SPEEDDIVIDE);
            yield return null;
        }
    }

    private IEnumerator MoveProjectile(Projectile projectile)
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

    private float GetTargetDistance(Enemy enemy)
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

    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach (var enemy in WaveController.Instance.Enemies)
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) < _currMaxAtackRadius)
            {
                if (Vector2.Distance(enemy.transform.position, transform.position) > _minAtackRadius)
                {
                    enemiesInRange.Add(enemy);
                }
            }
        }

        return enemiesInRange;
    }

    private Enemy GetNearestEnemy()
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

    public bool Upgrade()
    {
        if (_currLvl == 1)
        {
            _currDelayBtwAtack = _l2DelayBtwAtack;
            _currMaxAtackRadius = _l2MaxAtackRadius;
            _currSellCost = _l2SellCost;
            _currDamage = _l2Damage;
            _currDivisor = _l2Divisor;
            _currUpgradeCost = _l2UpgradeCost;
            //_spriteRenderer.sprite = _l2Sprite;

            _currLvl++;

            _animator.SetInteger("_currTowerLevel", _currLvl);

            return true;
        }
        else if (_currLvl == 2)
        {
            _currDelayBtwAtack = _l3DelayBtwAtack;
            _currMaxAtackRadius = _l3MaxAtackRadius;
            _currSellCost = _l3SellCost;
            _currDamage = _l3Damage;
            _currDivisor = _l3Divisor;
            _currUpgradeCost = _l3UpgradeCost;
            //_spriteRenderer.sprite = _l3Sprite;

            _currLvl++;

            _animator.SetInteger("_currTowerLevel", _currLvl);

            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _minAtackRadius);
        Gizmos.DrawWireSphere(transform.position, _currMaxAtackRadius);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_type == TowerType.Gravity)
        {
            if (collision.GetComponent<Enemy>())
            {
                //collision.GetComponent<Enemy>().SpeedDivide(_currDivisor);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_type == TowerType.Gravity)
        {
            if (collision.GetComponent<Enemy>())
            {
                //collision.GetComponent<Enemy>().ExitGravity();
            }
        }
    }
}
