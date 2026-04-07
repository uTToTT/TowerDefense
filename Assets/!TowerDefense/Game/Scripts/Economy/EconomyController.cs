using System;
using TMPro;
using UnityEngine;

public class EconomyController : IDisposable
{
    [SerializeField] private TMP_Text _balanceText;
    [SerializeField] private int _startMoney;

    private readonly IWalletView _walletView;
    private readonly Wallet _wallet;

    public EconomyController(Wallet wallet, IWalletView walletView)
    {
        _wallet = wallet;
        _walletView = walletView;

        SubOnEvents();
    }

    public void Restart()
    {
    }

    public bool CanSpend(double amount) => _wallet.CanSpend(amount);
    public void AddMoney(float amount) => _wallet.Add(amount);
    public bool Spend(int moneyAmount) => _wallet.Spend(moneyAmount);
   
    private void SubOnEvents()
    {
        _wallet.OnBalanceChanged += _walletView.SetBalance;
    }

    private void UnsubOnEvents()
    {
        _wallet.OnBalanceChanged -= _walletView.SetBalance;
    }

    public void Dispose()
    {
        UnsubOnEvents();
    }
}
