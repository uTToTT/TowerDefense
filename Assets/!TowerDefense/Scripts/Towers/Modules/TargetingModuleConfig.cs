using UnityEngine;

[CreateAssetMenu(fileName = "TargetingModuleConfig", menuName = "TD/Tower/Modules/Targeting config")]
public class TargetingModuleConfig : TowerModuleConfig
{
    [SerializeField, Range(0, 5)] private float _minRange;
    [SerializeField, Range(0, 15)] private float _maxRange;
    
    public float MinRange => _minRange;
    public float MaxRange => _maxRange;

    public override ModuleType ModuleType => ModuleType.Targeting;
}
