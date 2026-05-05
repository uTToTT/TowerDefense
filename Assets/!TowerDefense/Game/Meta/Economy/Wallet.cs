using System;

public class Wallet
{
    public event Action<double> OnBalanceChanged;

    private double _money;

    public double Money => _money;

    public Wallet()
    {

    }

    #region API

    public bool CanSpend(double amount) => _money >= amount && amount >= 0;

    public bool Spend(double amount)
    {
        if (!CanSpend(amount))
            return false;

        _money -= amount;

        if (amount != 0)
            OnBalanceChanged?.Invoke(amount);

        return true;
    }

    public void Add(double amount)
    {
        if (amount <= 0)
            return;

        _money += amount;

        if (amount != 0)
            OnBalanceChanged?.Invoke(amount);
    }

    #endregion
}
