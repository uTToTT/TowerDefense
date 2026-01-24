using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHP;
    [SerializeField] private int _currHp;
    [HorizontalLine]

    [SerializeField] private TextMeshProUGUI _hpText;

    public static Player Instance { get; private set; }
    public int CurrHP
    {
        get => _currHp;
        set
        {
            _currHp = Mathf.Max(value, 0);
            _hpText.text = "HP: " + _currHp.ToString();
        }
    }

    private void Start() => Init();

    public void Init()
    {
        Instance = this;
        CurrHP = _maxHP;
    }

    public void Heal(int amount) => CurrHP += amount;

    public void TakeDamage(int damage)
    {
        CurrHP -= damage;

        if (CurrHP <= 0)
            Defeat();
    }

    private void Defeat() => EventBus.GameOver?.Invoke();
}
