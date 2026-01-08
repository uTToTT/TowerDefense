using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable, IEntityLifecycle
{
    [Header("Basic")]
    [SerializeField, Min(0)] private int _hp;
    [SerializeField, Min(0)] private int _damage;
    [SerializeField, Min(0)] private int _numDropMoney;
    [SerializeField, Range(0, 1)] private float _armor;
    [SerializeField, Range(0, 1)] private float _minSpeedPersents;
    [Space]
    //[SerializeField] private Color _color;
    //private Color _colorInGravity = new Color(0, 255, 155);
    //[SerializeField] private Color _colorFreezeL1 = new Color(170, 240, 255);
    //[SerializeField] private Color _colorFreezeL2 = new Color(83, 225, 255);
    //[SerializeField] private Color _colorFreezeL3 = new Color(0, 210, 255);
    [Space]
    [SerializeField] private ParticleSystem _deathExmplosion;
    [SerializeField] private ParticleSystem _hitVFX;
    [SerializeField] private EnemyType _enemyType;
    [Space]
    [Header("Freezing")]
    [SerializeField, Min(0)] private int _maxFreezeStack;
    [SerializeField, Min(0)] private int _freezingTime;
    [Space]
    [Header("Test")]
    [SerializeField] private float TESTSPEED;
    [SerializeField] private float TESTARMOR;
    [SerializeField] private float TESTFREEZESTACK;
    [SerializeField] private float TESTFREEZEDEBUFF;
    [SerializeField] private float TESTGRAVITYDEBUFF;
    [SerializeField] private float TESTHP;
    [SerializeField] private int TESTMONEY;
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
    //[Space]
    //[SerializeField, Min(0)] private int _l5HPStep;
    //[SerializeField, Range(0, 0.2f)] private float _l5MoveSpeed;
    //[SerializeField] private Sprite _l5HPSprite;
    //[SerializeField] private Sprite _l5HPSpriteArmor;

    private Dictionary<int, float> _moneyBuffsByGravity = new Dictionary<int, float>();
    private Dictionary<int, float> _speedDebuffsByGravity = new Dictionary<int, float>();
    private Dictionary<int, float> _hpDebuffsByGravity = new Dictionary<int, float>();

    private Vector2 _moveDirection;
    //private Color _baseColor;
    private int _currStep;
    private int _stepMoneyAdded;
    private bool _inArmor;
    private bool _armorBreak;

    private bool _firstStepMoneyAdded;
    private bool _secondStepMoneyAdded;
    private bool _thirdStepMoneyAdded;

    private float _currMoneyMultiplier;
    private int _tmpMoneyDrop;

    private float _currArmor;
    private float _tmpArmor;

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
    public int HP => _hp;
    public float Speed => _currSpeed;
    public float Shield => _armor;
    public float CurrArmor => _currArmor;
    public bool MaxFreeze => _maxFreeze;

    public bool IsActive { get; set; }

    private void Start()
    {
        _secondFreezeStep = (_maxFreezeStack / 3);
        _thirdFreezeStep = (_maxFreezeStack / 3) * 2;

        //Debug.Log("second freeze step" + _secondFreezeStep);
        //Debug.Log("third freeze step" + _thirdFreezeStep);

        //_baseColor = _spriteRenderer.color;
        _tmpMoneyDrop = _numDropMoney;
        _currArmor = _armor;
        ResetLevelStep();
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
        TESTHP = _hp;
        TESTMONEY = _numDropMoney;
    }

    private void Move()
    {
        if (_moveDirection != Vector2.zero)
        {
            transform.Translate(_moveDirection * _currSpeed);
        }
    }

    public void SetMoveDirection(Direction dir)
    {
        if (dir == Direction.Up)
        {
            _moveDirection = Vector2.up;
        }
        else if (dir == Direction.Down)
        {
            _moveDirection = Vector2.down;
        }
        else if (dir == Direction.Left)
        {
            _moveDirection = Vector2.left;
        }
        else if (dir == Direction.Right)
        {
            _moveDirection = Vector2.right;
        }
        else
        {
            Debug.Log("Error! Uncorrect move direction!" + dir);
        }
    }

    public void Death()
    {
        if (_deathExmplosion != null)
        {
            Destroy(Instantiate(_deathExmplosion, transform.position, Quaternion.identity).gameObject, 3f);
        }

        int dropMoney = _numDropMoney;

        if (!DeathStrongDrop())
        {
            EventBus.AddMoney!.Invoke(dropMoney);
        }

        WaveController.Instance.UnregisterEnemy(this);
    }

    public void TakeDamageToArmor(float damageArmor)
    {
        if (_currArmor - damageArmor > 0)
        {
            _currArmor -= damageArmor;
        }
        else
        {
            _currArmor = 0;
            _armorBreak = true;
        }

        ResetLevelStep();
    }

    public void TakeDamage(int damage, float armorPiercing)
    {
        _tmpArmor = _currArmor;

        if (_currArmor - armorPiercing > 0)
        {
            _currArmor -= armorPiercing;
        }
        else
        {
            _currArmor = 0;
        }

        float tmp = damage;
        tmp = tmp * (1 - _currArmor);
        damage = (int)tmp;

        if (_isHPDisionByGravity)
        {
            damage = (int)((float)damage * _currHPDivisor);
        }

        if (_hp - damage > 0)
        {
            _hp -= damage;
        }
        else
        {
            Death();
            return;
        }

        if (_hitVFX != null)
        {
            _hitVFX.gameObject.SetActive(true);
            _hitVFX.Play();
        }

        _currArmor = _tmpArmor;

        ResetLevelStep();
    }

    private bool DeathStrongDrop()
    {
        int totalMoney = 0;
        int pieceMoney = 0;

        if (_enemyType == EnemyType.Heavy)
        {
            pieceMoney = _numDropMoney / 3;
        }
        else if (_enemyType == EnemyType.King)
        {
            pieceMoney = _numDropMoney / 4;
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

        int moneyDrop;

        if (_enemyType == EnemyType.Heavy)
        {
            moneyDrop = _numDropMoney / 3;
        }
        else if (_enemyType == EnemyType.King)
        {
            moneyDrop = _numDropMoney / 4;
        }
        else
        {
            return;
        }

        EventBus.AddMoney?.Invoke(moneyDrop);
    }

    private void ResetLevelStep()
    {
        if (_hp <= _l1HPStep && _hp >= _l2HPStep)
        {
            _currStep = 1;

            if (_currArmor <= 0f)
            {
                if (_l1HPSprite != null && _spriteRenderer.sprite != _l1HPSprite)
                {
                    _spriteRenderer.sprite = _l1HPSprite;
                    _inArmor = false;
                }
            }
            else
            {
                if (_l1HPSpriteArmor != null && _spriteRenderer.sprite != _l1HPSpriteArmor)
                {
                    _spriteRenderer.sprite = _l1HPSpriteArmor;
                    _inArmor = true;
                }
            }
            ChangeFreezeSprite();
            ResetSpeed(_l1MoveSpeed);
        }
        else if (_hp <= _l2HPStep && _hp >= _l3HPStep)
        {
            _currStep = 2;

            if (_currArmor <= 0f)
            {
                if (_l2HPSprite != null && _spriteRenderer.sprite != _l2HPSprite)
                {
                    _spriteRenderer.sprite = _l2HPSprite;
                    _inArmor = false;
                }
            }
            else
            {
                if (_l2HPSpriteArmor != null && _spriteRenderer.sprite != _l2HPSpriteArmor)
                {
                    _spriteRenderer.sprite = _l2HPSpriteArmor;
                    _inArmor = true;
                }
            }
            ChangeFreezeSprite();
            ResetSpeed(_l2MoveSpeed);
            MoneyStrongDrop();
            _firstStepMoneyAdded = true;
        }
        else if (_hp <= _l3HPStep && _hp >= _l4HPStep)
        {
            _currStep = 3;

            if (_currArmor <= 0f)
            {
                if (_l3HPSprite != null && _spriteRenderer.sprite != _l3HPSprite)
                {
                    _spriteRenderer.sprite = _l3HPSprite;
                    _inArmor = false;
                }
            }
            else
            {
                if (_l3HPSpriteArmor != null && _spriteRenderer.sprite != _l3HPSpriteArmor)
                {
                    _spriteRenderer.sprite = _l3HPSpriteArmor;
                    _inArmor = true;
                }
            }
            ChangeFreezeSprite();
            ResetSpeed(_l3MoveSpeed);
            MoneyStrongDrop();
            _secondStepMoneyAdded = true;
        }
        else if (_hp <= _l4HPStep /*&& _hp >= _l5HPStep*/)
        {
            _currStep = 4;

            if (_currArmor <= 0f)
            {
                if (_l4HPSprite != null && _spriteRenderer.sprite != _l4HPSprite)
                {
                    _spriteRenderer.sprite = _l4HPSprite;
                    _inArmor = false;
                }
            }
            else
            {
                if (_l4HPSpriteArmor != null && _spriteRenderer.sprite != _l4HPSpriteArmor)
                {
                    _spriteRenderer.sprite = _l4HPSpriteArmor;
                    _inArmor = true;
                }
            }
            ChangeFreezeSprite();
            ResetSpeed(_l4MoveSpeed);
            MoneyStrongDrop();
            _thirdStepMoneyAdded = true;
        }
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

        if (_freezeStack + freezeIncrement < _maxFreezeStack)
        {
            _freezeStack += freezeIncrement;
            _maxFreeze = false;
        }
        else
        {
            _freezeStack = _maxFreezeStack;
            _maxFreeze = true;
        }

        ChangeFreezeSprite();

        _calculateInUpdateCaleed = false;
        _timeToDefreeze = _freezingTime;
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
        else if (_totalDebuffSpeed < _minSpeedPersents)
        {
            _totalDebuffSpeed = _minSpeedPersents;
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

        _numDropMoney = (int)((float)_tmpMoneyDrop * _currMoneyMultiplier);
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

    public EnemyType GetEnemyType()
    {
        return _enemyType;
    }

    public int Getdamage()
    {
        return _damage;
    }

    public void HPMultiply(float multiplier)
    {
        float tmp = _hp;
        tmp *= multiplier;
        _hp = (int)tmp;

        tmp = _l1HPStep;
        tmp *= multiplier;
        _l1HPStep = (int)tmp;

        tmp = _l2HPStep;
        tmp *= multiplier;
        _l2HPStep = (int)tmp;

        tmp = _l3HPStep;
        tmp *= multiplier;
        _l3HPStep = (int)tmp;

        tmp = _l4HPStep;
        tmp *= multiplier;
        _l4HPStep = (int)tmp;
    }

    public void MoneyDropMultiply(float multipier)
    {
        _tmpMoneyDrop = _numDropMoney;
        float tmp = _numDropMoney;
        tmp *= multipier;
        _numDropMoney = (int)tmp;
        _tmpMoneyDrop = _numDropMoney;
    }

    public void MoveTo(Transform targetTransform)
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, targetTransform.position, _currSpeed * Time.deltaTime);
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
