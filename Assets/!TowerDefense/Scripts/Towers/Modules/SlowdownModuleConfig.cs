using UnityEngine;

[CreateAssetMenu(fileName = "SlowdownModuleConfig", menuName = "TD/Tower/Modules/Slowdown")]
public class SlowdownModuleConfig : TowerModuleConfig
{
    [Range(0, 1)] public float Force;
    [Range(0, 10)] public float Duration;

    public override ModuleType ModuleType => ModuleType.Slowdown;
}
