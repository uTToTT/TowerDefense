using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackModuleConfig", menuName = "TD/Tower/Modules/Attack")]
public class AttackModuleConfig : TowerModuleConfig
{
    [SerializeField, Range(0, 500)] private float _damage = 1;
    [SerializeField, Range(0.01f, 30)] private float _baseFireRate = 1;
    [SerializeField, Range(0f, 1f)] private float _piercing = 0;
    [HorizontalLine]

    [SerializeField, Min(0)] private float _rotationSpeed = 1;
    [SerializeField, Range(0, 360)] private float _aimThresholdDegress = 1;

    [Header("Effects")]
    [SerializeField] private ParticlesType _fireParticle = ParticlesType.TowerFire;
    [SerializeField] private SoundId _fireSound = SoundId.TowerShot;

    public float FireRate => _baseFireRate;
    public float Damage => _damage;
    public float Piercing => _piercing;
    public float RotationSpeed => _rotationSpeed;
    public float AimThresholdDegrees => _aimThresholdDegress;
    public ParticlesType FireParticle => _fireParticle;
    public SoundId FireSound => _fireSound;

    public override ModuleType ModuleType => ModuleType.Attack;
}
