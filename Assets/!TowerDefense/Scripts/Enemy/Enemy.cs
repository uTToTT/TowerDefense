using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable, IEntityLifecycle, IMovable
{
    [HorizontalLine]
    [SerializeField] private EnemyType _enemyType;

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
    [Header("Steps")]
    [SerializeField] private GameObject _l1FirstFreeze;
    [SerializeField] private GameObject _l1SecondFreeze;
    [SerializeField] private GameObject _l1ThirdFreeze;
    [Space]
    [SerializeField] private GameObject _l2FirstFreeze;
    [SerializeField] private GameObject _l2SecondFreeze;
    [SerializeField] private GameObject _l2ThirdFreeze;
    [Space]
    [SerializeField] private GameObject _l3FirstFreeze;
    [SerializeField] private GameObject _l3SecondFreeze;
    [SerializeField] private GameObject _l3ThirdFreeze;
    [Space]
    [SerializeField] private GameObject _l4FirstFreeze;
    [SerializeField] private GameObject _l4SecondFreeze;
    [SerializeField] private GameObject _l4ThirdFreeze;
    [Space]
    [Space]
    [SerializeField, Min(0)] private int _l1HPStep;
    [SerializeField, Range(0, 0.2f)] private float _l1MoveSpeed;
    [SerializeField] private Sprite _l1HPSprite;
    [SerializeField] private Sprite _l1HPSpriteArmor;
    [Space]
    [SerializeField, Min(0)] private int _l2HPStep;
    [SerializeField, Range(0, 0.2f)] private float _l2MoveSpeed;
    [SerializeField] private Sprite _l2HPSprite;
    [SerializeField] private Sprite _l2HPSpriteArmor;
    [Space]
    [SerializeField, Min(0)] private int _l3HPStep;
    [SerializeField, Range(0, 0.2f)] private float _l3MoveSpeed;
    [SerializeField] private Sprite _l3HPSprite;
    [SerializeField] private Sprite _l3HPSpriteArmor;
    [Space]
    [SerializeField, Min(0)] private int _l4HPStep;
    [SerializeField, Range(0, 0.2f)] private float _l4MoveSpeed;
    [SerializeField] private Sprite _l4HPSprite;
    [SerializeField] private Sprite _l4HPSpriteArmor;
    

    private Dictionary<int, float> _moneyBuffsByGravity = new Dictionary<int, float>();
    private Dictionary<int, float> _speedDebuffsByGravity = new Dictionary<int, float>();
    private Dictionary<int, float> _hpDebuffsByGravity = new Dictionary<int, float>();
    private List<Vector3> _routePoints = new();

    private int _currStep;
    private int _stepMoneyAdded;
    private bool _inArmor;
    private bool _armorBreak;

    private bool _firstStepMoneyAdded;
    private bool _secondStepMoneyAdded;
    private bool _thirdStepMoneyAdded;

    private float _currMoneyMultiplier;
    private float _tmpMoneyDrop;

    private float _currArmor;

    private float _tmpSpeed;
    private float _currSpeed;

    private float _freezeDebuff;
    private float _gravityDebuff;
    private float _totalDebuffSpeed;

    private bool _isSlowedByGravity;
    private bool _isMoneyMultipliedByGravity;
    private bool _calculateInUpdateCaleed;

    private bool _isHPDisionByGravity;
    private float _currHPDivisor;

    private bool _freezed;
    private bool _maxFreeze;
    private float _timeToDefreeze;
    private int _freezeStack;

    private int _secondFreezeStep;
    private int _thirdFreezeStep;

    public EnemyType EnemyType => _enemyType;
    public float CurrHP => _currHP;
    public float CurrArmor => _currArmor;
    public bool MaxFreeze => _maxFreeze;

    public bool IsActive { get; set; }

    private void Start() => Init();

    private void Init()
    {
        _secondFreezeStep = (_config.MaxFreezeStack / 3);
        _thirdFreezeStep = (_config.MaxFreezeStack / 3) * 2;

        //Debug.Log("second freeze step" + _secondFreezeStep);
        //Debug.Log("third freeze step" + _thirdFreezeStep);

        //_baseColor = _spriteRenderer.color;
        _tmpMoneyDrop = _config.DropMoney;
        _currArmor = _config.Armor;
        _hitVFX.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_timeToDefreeze > 0)
        {
            _timeToDefreeze -= Time.deltaTime;
        }
        else
        {
            _freezeStack = 0;
            _freezed = false;
            _maxFreeze = false;
            //_spriteRenderer.color = _baseColor;
            DisableAllFreezeSprite();

            if (!_calculateInUpdateCaleed)
            {
                _calculateInUpdateCaleed = true;
                Calculate—haracteristics();
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
        Calculate—haracteristics();

        TESTSPEED = _currSpeed;
        TESTARMOR = _currArmor;
        TESTFREEZESTACK = _freezeStack;
        TESTFREEZEDEBUFF = _freezeDebuff;
        TESTGRAVITYDEBUFF = _gravityDebuff;
        TESTHP = CurrHP;
        TESTMONEY = _currDropMoney;
    }

    public void Move()
    {
            transform.Translate(Vector2.left * _currSpeed);
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
            EventBus.AddMoney!.Invoke(dropMoney);
        }

        WaveController.Instance.UnregisterEnemy(this);
    }

    public void TakeDamageToArmor(float damageArmor)
    {
        _currArmor = Mathf.Max(0, _currArmor - damageArmor);
        _armorBreak = _currArmor <= 0;
    }

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

    private void MoneyStrongDrop()
    {
        if (_currStep == _stepMoneyAdded)
        {
            return;
        }

        _stepMoneyAdded = _currStep;

        float moneyDrop;

        if (_enemyType == EnemyType.Heavy)
        {
            moneyDrop = _currDropMoney / 3;
        }
        else if (_enemyType == EnemyType.King)
        {
            moneyDrop = _currDropMoney / 4;
        }
        else
        {
            return;
        }

        EventBus.AddMoney?.Invoke(moneyDrop);
    }

    private void ResetSpeed(float newSpeed)
    {
        _currSpeed = newSpeed;
        _tmpSpeed = newSpeed;
        _isSlowedByGravity = false;
        Calculate—haracteristics();
    }

    public void Freeze(int freezeIncrement)
    {
        _freezed = true;

        if (_freezeStack + freezeIncrement < _config.MaxFreezeStack)
        {
            _freezeStack += freezeIncrement;
            _maxFreeze = false;
        }
        else
        {
            _freezeStack = _config.MaxFreezeStack;
            _maxFreeze = true;
        }

        ChangeFreezeSprite();

        _calculateInUpdateCaleed = false;
        _timeToDefreeze = _config.FreezingTime;
        Calculate—haracteristics();
    }

    private void ChangeFreezeSprite()
    {
        DisableAllFreezeSprite();

        if (_freezeStack > 0 && _freezeStack < _secondFreezeStep)
        {
            if (_currStep == 1)
            {
                _l1FirstFreeze.SetActive(true);
            }
            else if (_currStep == 2)
            {
                _l2FirstFreeze.SetActive(true);
            }
            else if (_currStep == 3)
            {
                _l3FirstFreeze.SetActive(true);
            }
            else if (_currStep == 4)
            {
                _l4FirstFreeze.SetActive(true);
            }

            //Debug.Log("First freeze step " + _freezeStack);
        }
        else if (_freezeStack >= _secondFreezeStep && _freezeStack < _thirdFreezeStep)
        {
            if (_currStep == 1)
            {
                _l1SecondFreeze.SetActive(true);
            }
            else if (_currStep == 2)
            {
                _l2SecondFreeze.SetActive(true);
            }
            else if (_currStep == 3)
            {
                _l3SecondFreeze.SetActive(true);
            }
            else if (_currStep == 4)
            {
                _l4SecondFreeze.SetActive(true);
            }

            //Debug.Log("Second freeze step " + _freezeStack);
        }
        else if (_freezeStack >= _thirdFreezeStep)
        {
            if (_currStep == 1)
            {
                _l1ThirdFreeze.SetActive(true);
            }
            else if (_currStep == 2)
            {
                _l2ThirdFreeze.SetActive(true);
            }
            else if (_currStep == 3)
            {
                _l3ThirdFreeze.SetActive(true);
            }
            else if (_currStep == 4)
            {
                _l4ThirdFreeze.SetActive(true);
            }

            //Debug.Log("Third freeze step " + _freezeStack);
        }
    }

    private void DisableAllFreezeSprite()
    {
        if (_l1FirstFreeze != null)
        {
            _l1FirstFreeze.SetActive(false);
        }
        if (_l1SecondFreeze != null)
        {
            _l1SecondFreeze.SetActive(false);
        }
        if (_l1ThirdFreeze != null)
        {
            _l1ThirdFreeze.SetActive(false);
        }

        if (_l2FirstFreeze != null)
        {
            _l2FirstFreeze.SetActive(false);
        }
        if (_l2SecondFreeze != null)
        {
            _l2SecondFreeze.SetActive(false);
        }
        if (_l2ThirdFreeze != null)
        {
            _l2ThirdFreeze.SetActive(false);
        }

        if (_l3FirstFreeze != null)
        {
            _l3FirstFreeze.SetActive(false);
        }
        if (_l3SecondFreeze != null)
        {
            _l3SecondFreeze.SetActive(false);
        }
        if (_l3ThirdFreeze != null)
        {
            _l3ThirdFreeze.SetActive(false);
        }

        if (_l4FirstFreeze != null)
        {
            _l4FirstFreeze.SetActive(false);
        }
        if (_l4SecondFreeze != null)
        {
            _l4SecondFreeze.SetActive(false);
        }
        if (_l4ThirdFreeze != null)
        {
            _l4ThirdFreeze.SetActive(false);
        }
    }

    private void CalculateSpeed()
    {
        float maxSpeedDivisor = 0;

        if (_speedDebuffsByGravity.Count != 0)
        {
            foreach (var item in _speedDebuffsByGravity)
            {
                if (item.Value > maxSpeedDivisor)
                {
                    maxSpeedDivisor = item.Value;
                }
            }

            _isSlowedByGravity = true;
        }
        else
        {
            _isSlowedByGravity = false;
        }

        _freezeDebuff = _freezed ? Mathf.Clamp01(_freezeStack / 100f) : 0;
        _gravityDebuff = _isSlowedByGravity ? Mathf.Clamp01(maxSpeedDivisor) : 0;

        _totalDebuffSpeed = Mathf.Clamp01(1 - (_freezeDebuff + _gravityDebuff));

        //Debug.Log("Freeze debuff: " + _freezeDebuff);
        //Debug.Log("Gravity debuff: " + _gravityDebuff);
        //Debug.Log("Total debuff: " + _totalDebuffSpeed);

        if (_totalDebuffSpeed > 1)
        {
            _totalDebuffSpeed = 1;
        }
        else if (_totalDebuffSpeed < _config.MinSpeed)
        {
            _totalDebuffSpeed = _config.MinSpeed;
        }

        _currSpeed = _tmpSpeed * _totalDebuffSpeed;
    }

    private void CalculateDropMoney()
    {
        float maxMoneyMultiplier = 0;

        if (_moneyBuffsByGravity.Count != 0)
        {
            foreach (var item in _moneyBuffsByGravity)
            {
                if (item.Value > maxMoneyMultiplier)
                {
                    maxMoneyMultiplier = item.Value;
                }
            }

            _isMoneyMultipliedByGravity = true;
        }
        else
        {
            _isMoneyMultipliedByGravity = false;
        }

        _currMoneyMultiplier = _isMoneyMultipliedByGravity ? maxMoneyMultiplier : 1;

        _currDropMoney = (int)((float)_tmpMoneyDrop * _currMoneyMultiplier);
    }

    private void CalculateHP()
    {
        float maxHPDivisor = 0;

        if (_hpDebuffsByGravity.Count != 0)
        {
            foreach (var item in _hpDebuffsByGravity)
            {
                if (item.Value > maxHPDivisor)
                {
                    maxHPDivisor = item.Value;
                }
            }

            _isHPDisionByGravity = true;
        }
        else
        {
            _isHPDisionByGravity = false;
        }

        _currHPDivisor = _isHPDisionByGravity ? maxHPDivisor : 1;

        //_hp = (int)((float)_tmpHP / _currHPDivisor);
    }


    private void Calculate—haracteristics()
    {
        CalculateSpeed();
        CalculateDropMoney();
        CalculateHP();
    }

    public void EnterGravity(int indexTower, Gravity gravity)
    {
        //_spriteRenderer.color = _colorInGravity;

        if (!_speedDebuffsByGravity.ContainsKey(indexTower))
        {
            _speedDebuffsByGravity.Add(indexTower, gravity.CurrSpeedDivisor);
        }
        else if (_speedDebuffsByGravity.ContainsKey(indexTower) && _speedDebuffsByGravity[indexTower] != gravity.CurrSpeedDivisor)
        {
            _speedDebuffsByGravity[indexTower] = gravity.CurrSpeedDivisor;
        }

        if (gravity.SpecTypeGravity == SpecTypeGravity.MoneyMultyplier)
        {
            if (!_moneyBuffsByGravity.ContainsKey(indexTower))
            {
                _moneyBuffsByGravity.Add(indexTower, gravity.CurrMoneyDropMultiplier);
            }
            else if (_moneyBuffsByGravity.ContainsKey(indexTower) && _moneyBuffsByGravity[indexTower] != gravity.CurrMoneyDropMultiplier)
            {
                _moneyBuffsByGravity[indexTower] = gravity.CurrMoneyDropMultiplier;
            }
        }
        else if (gravity.SpecTypeGravity == SpecTypeGravity.HpDivisor)
        {
            if (!_hpDebuffsByGravity.ContainsKey(indexTower))
            {
                _hpDebuffsByGravity.Add(indexTower, gravity.CurrHealPointDivisor);
            }
            else if (_hpDebuffsByGravity.ContainsKey(indexTower) && _hpDebuffsByGravity[indexTower] != gravity.CurrHealPointDivisor)
            {
                _hpDebuffsByGravity[indexTower] = gravity.CurrHealPointDivisor;
            }
        }

        Calculate—haracteristics();
    }

    public void ExitGravity(int indexTower)
    {
        _moneyBuffsByGravity.Remove(indexTower);
        _speedDebuffsByGravity.Remove(indexTower);
        _hpDebuffsByGravity.Remove(indexTower);

        Calculate—haracteristics();
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
        //throw new System.NotImplementedException();
    }

    public void OnActivated()
    {
        //throw new System.NotImplementedException();
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

    //private void OnBecameInvisible()
    //{
    //    Death();
    //    Debug.Log("Invisible");
    //}
}



public enum EnemyType
{
    Classic,
    Fast,
    Armor,
    Heavy,
    King
}
