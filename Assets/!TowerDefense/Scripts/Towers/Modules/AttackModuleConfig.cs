using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/Tower/Modules/Attack")]
public class AttackModuleConfig : TowerModuleConfig
{
    [SerializeField, Range(0, 6)] private float _minRange;
    [SerializeField, Range(0, 30)] private float _maxRange;
    [HorizontalLine]

    [SerializeField, Range(0, 500)] private float _damage = 1;
    [SerializeField, Min(0.01f)] private float _fireRate = 1;
    [SerializeField, Range(0f, 1f)] private float _piercing = 0;
    [HorizontalLine]

    [SerializeField,Min(0)] private float _rotationSpeed = 1;

    public float MinRange => _minRange;
    public float MaxRange => _maxRange;
    public float FireRate => _fireRate;
    public float Damage => _damage;
    public float Piercing => _piercing;
    public float RotationSpeed => _rotationSpeed;
}
