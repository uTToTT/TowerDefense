using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _balanceText;

    public static EconomyManager Instance { get; private set; }

    private float _balance;

    public void Init()
    {
        Instance = this;
        SetBalance(0);
    }

    public bool CanSpend(int moneyAmoount) => _balance - moneyAmoount >= 0;
    public void AddMoney(float amount) => SetBalance(_balance + amount);

    public bool Spend(int moneyAmount)
    {
        if (CanSpend(moneyAmount))
        {
            SetBalance(_balance - moneyAmount);
            return true;
        }

        return false;
    }

    private void SetBalance(float amount)
    {
        _balance = Mathf.Max(amount, 0);
        _balanceText.text = "Money: " + _balance.ToString();
    }
}
