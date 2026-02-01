using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "EnergyModuleConfig", menuName = "TD/Tower/Modules/Energy")]
public sealed class EnergyModuleConfig : TowerModuleConfig
{
    [HorizontalLine]
    [Range(0, 25)] public float EnergyProduction = 0;
    [Range(0, 25)] public float EnergyConsumpition = 0;

    public override ModuleType ModuleType => ModuleType.Energy;
}
