using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour,
    IPoolable, IEntityLifecycle, IMovable, IBuffable
{
    public event Action<Enemy> OnDeath;

    [HorizontalLine]
    [SerializeField] private EnemyType _enemyType;
    [SerializeField, Range(0, 1)] private float _laneOffset;

    [HorizontalLine]
    [Expandable]
    [SerializeField] private EnemyConfig _config;


    [Space]
    [SerializeField] private ParticleSystem _deathExmplosion;
    [SerializeField] private ParticleSystem _hitVFX;

    private float _currHP;
    private float _currDropMoney;

    [Space]
    [Header("Test")]
    [SerializeField] private float _remainingDistance;
    [SerializeField] private float TESTSPEED;
    [SerializeField] private float TESTARMOR;
    [SerializeField] private float TESTFREEZESTACK;
    [SerializeField] private float TESTFREEZEDEBUFF;
    [SerializeField] private float TESTHP;
    [SerializeField] private float TESTMONEY;
    [SerializeField] private float TESTDISTANCE;
    [Space]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [Space]

    private float _currArmor;
    private float _currSpeed;

    private PathLane _lane;

    private BuffController _buffController;
    private PathController _pathController;
    public EnemyType EnemyType => _enemyType;
    public float CurrHP => _currHP;
    public float MaxHp => _currHP;
    public float CurrSpeed =>
        BuffController.Calculate(Characteristics.SPEED, _config.Speed);
    public float CurrArmor => _currArmor;
    public bool IsActive { get; set; }
    public bool IsAlive { get; private set; }
    public BuffController BuffController => _buffController;

    private List<Vector3> _points;
    public float RemainingDistance => _pathController.RemainingDistance;
    public void Init()
    {
        _buffController = new BuffController();
        _pathController = new PathController();
        _currArmor = _config.Armor;
        _currSpeed = _config.Speed;
        _currHP = _config.HP;
    }

    public void Tick()
    {
        if (!IsActive || !IsAlive) return;
        Move();
    }

    private void OnValidate()
    {
        TESTSPEED = _currSpeed;
        TESTARMOR = _currArmor;
        TESTHP = CurrHP;
        TESTMONEY = _currDropMoney;
    }

    public void SetLane(PathLane lane) => _lane = lane;

    public void BuildRoute(List<Vector3> points)
    {
        _points = PathController.OffsetPath(points, _laneOffset, true);
        _pathController.SetPath(_points, transform.position);
        _pathController.OnFinishReached += () => HitPlayer();

        MoveManager.Instance.Register(this);
    }


    private void HitPlayer() => Player.Instance.TakeDamage(_config.Damage);

    public void Move()
    {
        if (!IsActive || !IsAlive) return;

        _remainingDistance = _pathController.RemainingDistance;
        _pathController.Advance(transform.position);

        if (!_pathController.HasPath)
        {
            MoveManager.Instance.Unregister(this);
            return;
        }

        Vector3 target = _pathController.Peek();

        transform.MoveTowards(target, _config.Speed);
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

    public void Death()
    {
        EventBus.AddMoney?.Invoke(_currDropMoney);
        WaveController.Instance.UnregisterEnemy(this);

        OnDeath?.Invoke(this);
    }

    public void TakeDamageToArmor(float damageArmor) =>
        _currArmor = Mathf.Max(0, _currArmor - damageArmor);

    public void TakeDamage(float damage, float armorPiercing)
    {
        var tmpArmor = _currArmor;

        _currArmor = Mathf.Max(0, _currArmor - armorPiercing);

        damage *= (1 - CurrArmor);

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

    public EnemyType GetEnemyType() => _enemyType;

    public float GetDamage() => _config.Damage;

    public void HPMultiply(float multiplier) =>
        _currHP *= multiplier;

    public void MoneyDropMultiply(float multipier) =>
        _currDropMoney *= multipier;

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
        IsAlive = true;
    }

    public void OnDeactivated()
    {
        //throw new System.NotImplementedException();
    }

    public void OnReturned()
    {
        IsAlive = false;
        _pathController.Clear();
        //throw new System.NotImplementedException();
    }

    public void OnDestroyed()
    {
        //throw new System.NotImplementedException();
    }


}



