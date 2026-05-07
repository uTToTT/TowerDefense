using System;
using TToTT.TowerDefense.UI;

public class EconomyController : IDisposable
{
    private readonly IWalletView _walletView;
    private readonly Wallet _wallet;

    public EconomyController(Wallet wallet, IWalletView walletView)
    {
        _wallet = wallet;
        _walletView = walletView;

        _wallet.OnBalanceChanged += _walletView.SetBalance;
    }

    public void Restart()
    {
        _wallet.Spend(_wallet.Money);
    }

    public bool CanSpend(double amount) => _wallet.CanSpend(amount);
    public void AddMoney(float amount) => _wallet.Add(amount);
    public bool Spend(int moneyAmount) => _wallet.Spend(moneyAmount);
   
    public void Dispose()
    {
        _wallet.OnBalanceChanged -= _walletView.SetBalance;
    }
}
