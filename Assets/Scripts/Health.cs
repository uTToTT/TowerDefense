using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _hp;
    [SerializeField] private TextMeshProUGUI _textHealth;

    private int _currHp;

    private void Start()
    {
        _currHp = _hp;
        _textHealth.text = _currHp.ToString();
    }

    private void HitBase(int damage)
    {
        if (_currHp - damage > 0)
        {
            _currHp -= damage;
            _textHealth.text = _currHp.ToString();
        }
        else
        {
            Defeat();
        }
    }

    public int GetHP()
    {
        return _currHp;
    }

    private void Defeat()
    {
        _textHealth.text = "0";
        EventBus.GameOver?.Invoke();
    }

    private void AddHp(int hp)
    {
        _currHp += 10;
        _textHealth.text = _currHp.ToString();
    }

    private void RewardAid()
    {
        AddHp(10);
    }

    private void OnEnable()
    {
        EventBus.onAid += RewardAid;
        EventBus.HitBase += HitBase;
    }

    private void OnDisable()
    {
        EventBus.onAid -= RewardAid;
        EventBus.HitBase -= HitBase;
    }
}
