using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour, IWalletView
{
    [SerializeField] private TMP_Text _balance;

    public void SetBalance(double amount)
    {
        _balance.text = "Money: " + (amount).ToString();
    }
}
