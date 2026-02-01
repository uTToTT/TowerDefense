using NaughtyAttributes;
using UnityEngine;

public abstract class TowerModuleConfig : ScriptableObject
{
    [HorizontalLine]
    [Range(0, 25)] public float EnergyProduction = 0;
    [Range(0, 25)] public float EnergyConsumpition = 0;

    [HorizontalLine]
    [Range(0f, 1f)] public float MinEnergyCoef = 0;
    [Range(0f, 50f)] public float MaxEnergyCoef = 1;

    public abstract ModuleType ModuleType { get; }
}
