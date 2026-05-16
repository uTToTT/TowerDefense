using NUnit.Framework;
using TToTT.TowerDefense.Economy;

public class WalletTests
{
    private Wallet _wallet;

    [SetUp]
    public void Setup() => _wallet = new Wallet();

    [Test]
    public void Add_IncreasesBalance()
    {
        _wallet.Add(100);
        Assert.AreEqual(100, _wallet.Money);
    }

    [Test]
    public void Spend_DecreasesBalance()
    {
        _wallet.Add(100);
        _wallet.Spend(40);
        Assert.AreEqual(60, _wallet.Money);
    }

    [Test]
    public void CanSpend_ReturnsFalse_WhenNotEnoughMoney()
    {
        _wallet.Add(50);
        Assert.IsFalse(_wallet.CanSpend(100));
    }

    [Test]
    public void Spend_ReturnsFalse_WhenNotEnoughMoney()
    {
        _wallet.Add(50);
        var result = _wallet.Spend(100);
        Assert.IsFalse(result);
        Assert.AreEqual(50, _wallet.Money, "Balance must not change");
    }

    [Test]
    public void Add_FiresOnBalanceChanged()
    {
        double received = -1;
        _wallet.OnBalanceChanged += val => received = val;

        _wallet.Add(75);

        Assert.AreEqual(75, received);
    }

    [Test]
    public void Add_Zero_DoesNotFireEvent()
    {
        bool fired = false;
        _wallet.OnBalanceChanged += _ => fired = true;

        _wallet.Add(0);

        Assert.IsFalse(fired, "Event must not occur when 0 is added");
    }
}