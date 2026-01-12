using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,
    IPoolable, IEntityLifecycle, IMovable, IBuffable
{
    public event Action<Enemy> OnDeath;
    public event Action<Enemy> PathEnd;

    [HorizontalLine]
    [SerializeField] private EnemyType _enemyType;
    [SerializeField, Range(0, 1)] private float _laneOffset;

    [HorizontalLine]
    [SerializeField] private EnemyConfig _config;


    [Space]
    [SerializeField] private ParticleSystem _deathExmplosion;
    [SerializeField] private ParticleSystem _hitVFX;

    private float _currHP;
    private float _currDropMoney;

    [Space]
    [Header("Test")]
    [SerializeField] private float TESTSPEED;
    [SerializeField] private float TESTARMOR;
    [SerializeField] private float TESTFREEZESTACK;
    [SerializeField] private float TESTFREEZEDEBUFF;
    [SerializeField] private float TESTGRAVITYDEBUFF;
    [SerializeField] private float TESTHP;
    [SerializeField] private float TESTMONEY;
    [Space]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [Space]

    private bool _firstStepMoneyAdded;
    private bool _secondStepMoneyAdded;
    private bool _thirdStepMoneyAdded;
    private Vector3 _segmentTarget;
    private Vector3 _segmentFrom;


    private float _currArmor;

    private float _currSpeed;

    private float _gravityDebuff;

    private bool _isHPDisionByGravity;
    private float _currHPDivisor;

    private PathLane _lane;

    private BuffController _buffController;
    private PathController _pathController;
    public EnemyType EnemyType => _enemyType;
    public float CurrHP => _currHP;
    public float CurrSpeed =>
        BuffController.Calculate(Characteristics.SPEED, _config.Speed);
    public float CurrArmor => _currArmor;
    public bool IsActive { get; set; }

    public BuffController BuffController => _buffController;

    private List<Vector3> _points;

    public void Init()
    {
        _buffController = new BuffController();
        _pathController = new PathController();
        _currArmor = _config.Armor;
        _currSpeed = _config.Speed;
    }

    public void Tick()
    {
        Move();
    }

    private void OnValidate()
    {
        TESTSPEED = _currSpeed;
        TESTARMOR = _currArmor;
        TESTGRAVITYDEBUFF = _gravityDebuff;
        TESTHP = CurrHP;
        TESTMONEY = _currDropMoney;
    }

    public void SetLane(PathLane lane) => _lane = lane;

    public void BuildRoute(List<Vector3> points)
    {
        string ps = string.Empty;

        _points = PathController.OffsetPath(points, _laneOffset, true);

        foreach (var p in _points)
        {
            _pathController.Enqueue(p);
        }

        MoveManager.Instance.Register(this);
        RecalculateSegmentTarget();
    }

    public void Move()
    {
        if (!_pathController.HasPath)
        {
            PathEnd?.Invoke(this);
            MoveManager.Instance.Unregister(this);
            return;
        }

        Vector3 target = _pathController.Peek();

        transform.MoveTowards(target, _config.Speed);

        if (transform.IsReach(target))
        {
            _pathController.Dequeue();
        }
    }

    private void OnDrawGizmos()
    {
        if (_points == null || _points.Count == 0)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < _points.Count; i++)
        {
            Gizmos.DrawSphere(_points[i], 0.1f);

            if (i < _points.Count - 1)
            {
                Gizmos.DrawLine(_points[i], _points[i + 1]);
            }
        }
    }

    private void RecalculateSegmentTarget()
    {
        _segmentFrom = transform.position;
        Vector3 target = _pathController.Peek();

        Vector3 dir = (target - _segmentFrom).normalized;
        Vector3 perpendicular = new Vector3(-dir.y, dir.x, 0f);

        Vector3 offset = _lane switch
        {
            PathLane.Left => perpendicular * _laneOffset,
            PathLane.Right => -perpendicular * _laneOffset,
            _ => Vector3.zero
        };

        _segmentTarget = target + offset;
    }


    public void Death()
    {
        if (_deathExmplosion != null)
        {
            Destroy(Instantiate(_deathExmplosion, transform.position, Quaternion.identity).gameObject, 3f);
        }

        float dropMoney = _currDropMoney;

        if (!DeathStrongDrop())
        {
            EventBus.AddMoney?.Invoke(dropMoney);
        }

        WaveController.Instance.UnregisterEnemy(this);
    }

    public void TakeDamageToArmor(float damageArmor) =>
        _currArmor = Mathf.Max(0, _currArmor - damageArmor);

    public void TakeDamage(float damage, float armorPiercing)
    {
        var tmpArmor = _currArmor;

        _currArmor = Mathf.Max(0, _currArmor - armorPiercing);

        damage = damage * (1 - CurrArmor);

        if (_isHPDisionByGravity)
        {
            damage *= _currHPDivisor;
        }

        _currHP = Mathf.Max(_currHP - damage, 0);

        if (_currHP <= 0)
        {
            Death();
            return;
        }

        if (_hitVFX != null)
        {
            _hitVFX.gameObject.SetActive(true);
            _hitVFX.Play();
        }

        _currArmor = tmpArmor;
    }

    private bool DeathStrongDrop()
    {
        float totalMoney = 0;
        float pieceMoney = 0;

        if (_enemyType == EnemyType.Heavy)
        {
            pieceMoney = _currDropMoney / 3;
        }
        else if (_enemyType == EnemyType.King)
        {
            pieceMoney = _currDropMoney / 4;
        }
        else
        {
            return false;
        }

        if (!_firstStepMoneyAdded)
        {
            totalMoney += pieceMoney;
        }
        if (!_secondStepMoneyAdded)
        {
            totalMoney += pieceMoney;
        }
        if (!_thirdStepMoneyAdded && _enemyType != EnemyType.Heavy)
        {
            totalMoney += pieceMoney;
        }

        totalMoney += pieceMoney;

        EventBus.AddMoney?.Invoke(totalMoney);
        return true;
    }


    public void EnterGravity(int indexTower, Gravity gravity)
    {

    }

    public void ExitGravity(int indexTower)
    {

    }

    public EnemyType GetEnemyType() => _enemyType;

    public float GetDamage() => _config.Damage;

    public void HPMultiply(float multiplier) =>
        _currHP *= multiplier;

    public void MoneyDropMultiply(float multipier) =>
        _currDropMoney *= multipier;

    public void MoveTo(Transform targetTransform)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, _currSpeed * Time.deltaTime);
    }

    public void Dispose()
    {
        //throw new System.NotImplementedException();
    }

    public void OnPreload()
    {
    }

    public void OnActivated()
    {
        Init();

    }

    public void OnDeactivated()
    {
        //throw new System.NotImplementedException();
    }

    public void OnReturned()
    {
        //throw new System.NotImplementedException();
    }

    public void OnDestroyed()
    {
        //throw new System.NotImplementedException();
    }


}



