using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour, IPlayerTarget
{
    [SerializeField] private CameraShaker _shaker;

    [SerializeField] private float _maxHP;
    [SerializeField] private float _currHp;
    [HorizontalLine]

    [SerializeField] private TextMeshProUGUI _hpText;

    public static Player Instance { get; private set; }
    public float CurrHP
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

    public void Restart()
    {
        CurrHP = _maxHP;
    }

    public void Heal(int amount) => CurrHP += amount;

    public void TakeDamage(float damage)
    {
        CurrHP -= damage;
        _shaker.Shake();
        if (CurrHP <= 0)
            Defeat();
    }

    private void Defeat() => throw new NotImplementedException();

    //GameLoop.Instance.PlayerBaseDestroyed();
}
