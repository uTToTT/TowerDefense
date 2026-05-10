using System;
using TToTT.TowerDefense.UI.Label;
using UnityEngine;

public class Player : IPlayerTarget
{
    public event Action OnPlayerDie;

    private readonly CameraShaker _shaker;
    private readonly ILabelView _hpBar;

    private float _maxHP = 20;
    private float _currHp;

    // TODO: create reactive class include value bounds
    public float CurrHP
    {
        get => _currHp;
        set
        {
            _currHp = Mathf.Max(value, 0);
            _hpBar.SetText($"HP {_currHp.ToString()}");
        }
    }

    // TODO: replace shaker to VFX layer
    public Player(CameraShaker cameraShaker, LabelRegistry labelRegistry)
    {
        _shaker = cameraShaker;
        _hpBar = labelRegistry.Get(LabelId.PlayerHP);
        Restart();
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

    private void Defeat() => OnPlayerDie?.Invoke();
}
