using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "TD/Enemy/Enemy config")]
public class EnemyConfig : ScriptableObject
{
    [HorizontalLine]
    [SerializeField, Min(1)] private float _hp;
    [SerializeField, Range(0, 1)] private float _armor;
    [SerializeField, Min(0)] private float _damage;
    [Space]
    [SerializeField, Range(0, 25)] private float _speed = 0.01f;
    [SerializeField, Range(0, 1)] private float _minSpeed = 0.01f;
    [Space]
    [SerializeField, Range(0, 100)] private float _dropMoney;

    [HorizontalLine]
    [SerializeField, Range(0, 50)] private int _maxFreezeStack;
    [SerializeField, Range(0, 20)] private float _freezingTime;


    public float HP => _hp;
    public float Armor => _armor;
    public float Damage => _damage;
    public float Speed => _speed;
    public float MinSpeed => _minSpeed;
    public float DropMoney => _dropMoney;
    public int MaxFreezeStack => _maxFreezeStack;
    public float FreezingTime => _freezingTime;
}
