using UnityEngine;

public class Tower : MonoBehaviour, IPoolable, IEntityLifecycle
{
    [SerializeField] protected TowerType _towerType;
    [SerializeField] protected int _uniqueTowerIndex;
    [SerializeField] protected int _currlevelTEST;
    [Space]
    [Header("Prices")]
    [SerializeField, Min(0)] protected int _l1Price;
    [SerializeField, Min(0)] protected int _l1SellCost;
    [Space]
    [SerializeField, Min(0)] protected int _l2Price;
    [SerializeField, Min(0)] protected int _l2SellCost;
    [Space]
    [SerializeField, Min(0)] protected int _l3FirstSpecPrice;
    [SerializeField, Min(0)] protected int _l3FirstSpecSellCost;
    [Space]
    [SerializeField, Min(0)] protected int _l3SecondSpecPrice;
    [SerializeField, Min(0)] protected int _l3SecondSpecSellCost;
    [Space]
    [SerializeField, Min(0)] protected int _l4FirstSpecPrice;
    [SerializeField, Min(0)] protected int _l4FirstSpecSellCost;
    [Space]
    [SerializeField, Min(0)] protected int _l4SecondSpecPrice;
    [SerializeField, Min(0)] protected int _l4SecondSpecSellCost;
    [Space]
    [Space]

    protected int _currPrice;
    protected int _currSellCost;
    protected int _currLevel;

    protected bool _hasFirstSpec;
    protected bool _hasSecondSpec;

    public TowerType TowerType => _towerType;
    public int UniqueTowerIndex => _uniqueTowerIndex;

    public int L1Price => _l1Price;
    public int L1SellCost => _l1SellCost;

    public int L2Price => _l2Price;
    public int L2SellCost => _l2SellCost;

    public int L3FirstSpecPrice => _l3FirstSpecPrice;
    public int L3FirstSpecSellCost => _l3FirstSpecSellCost;

    public int L3SecondSpecPrice => _l3SecondSpecPrice;
    public int L3SecondSpecSellCost => _l3SecondSpecSellCost;

    public int L4FirstSpecPrice => _l4FirstSpecPrice;
    public int L4FirstSpecSellCost => _l4FirstSpecSellCost;

    public int L4SecondSpecPrice => _l4SecondSpecPrice;
    public int L4SecondSpecSellCost => _l4SecondSpecSellCost;

    public int CurrPrice => _currPrice;
    public int CurrSellCost => _currSellCost;
    public int CurrLevel => _currLevel;

    public bool HasFirstSpec => _hasFirstSpec;
    public bool HasSecondSpec => _hasSecondSpec;

    public bool IsActive { get; set; }

    void Start()
    {
        UpgradeTower();
    }

    protected bool UpgradeTower()
    {
        _currlevelTEST = _currLevel + 1;

        if (_currLevel >= 0 && _currLevel < 4)
        {
            _currLevel++;
        }

        if (_currLevel == 1)
        {
            _currPrice = _l2Price;
            _currSellCost = _l1SellCost;

            return true;
        }
        else if (_currLevel == 2)
        {
            _currPrice = -1;
            _currSellCost = _l2SellCost;

            return true;
        }
        else if (_hasFirstSpec)
        {
            if (_currLevel == 3)
            {
                _currSellCost = _l3FirstSpecSellCost;

                _currPrice = _l4FirstSpecPrice;

                return true;
            }
            else if (_currLevel == 4)
            {
                _currSellCost = _l4FirstSpecSellCost;

                return true;
            }
        }
        else if (_hasSecondSpec)
        {
            if (_currLevel == 3)
            {
                _currSellCost = _l3SecondSpecSellCost;

                _currPrice = _l4SecondSpecPrice;

                return true;
            }
            else if (_currLevel == 4)
            {
                _currSellCost = _l4SecondSpecSellCost;

                return true;
            }
        }

        _currlevelTEST = _currLevel + 1;
        return false;
    }

    public int GetSpecPrice(int index)
    {
        if (index == 1)
        {
            return _l3FirstSpecPrice;
        }
        else if (index == 2)
        {
            return _l3SecondSpecPrice;
        }

        return 0;
    }

    public void SetUniqueTowerIndex(int index)
    {
        _uniqueTowerIndex = index;
    }

    public void Dispose()
    {
    }

    public void OnPreload()
    {
    }

    public void OnActivated()
    {
    }

    public void OnDeactivated()
    {
    }

    public void OnReturned()
    {
    }

    public void OnDestroyed()
    {
    }
}

public enum TowerType
{
    Minigun,
    Twiin,
    Gravity,
    Rail
}

public enum SpecTypeMinigun
{
    None,
    Explosion,
    Freeze,
}

public enum SpecTypeTwiin
{
    None,
    TwoToOneAtack,
    Shard,
}

public enum SpecTypeGravity
{
    None,
    MoneyMultyplier,
    HpDivisor,
}

public enum SpecTypeRail
{
    None,
    Critical,
    BreakArmor,
}