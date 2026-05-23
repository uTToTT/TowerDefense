using System;
using TToTT.TowerDefense.UI.Label;

namespace TToTT.TowerDefense.Economy
{
    public class EconomyController : IDisposable
    {
        private readonly ILabelView _moneyText;
        private readonly Wallet _wallet;

        public EconomyController(Wallet wallet, LabelRegistry labels)
        {
            _wallet = wallet;
            _moneyText = labels.Get(LabelId.Money);

            _wallet.OnBalanceChanged += UpdateLabel;
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
            _wallet.OnBalanceChanged -= UpdateLabel;
        }

        private void UpdateLabel(double amount)
        {
            _moneyText.SetText(amount, "N0");
        }
    }
}