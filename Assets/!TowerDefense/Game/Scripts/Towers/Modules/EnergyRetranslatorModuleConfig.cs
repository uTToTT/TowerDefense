using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "EnergyRetranslatorModuleConfig", menuName = "TD/Tower/Modules/Energy Retranslator")]
public sealed class EnergyRetranslatorModuleConfig : TowerModuleConfig
{
    [HorizontalLine]
    public int TranslationRadius;

    public override ModuleType ModuleType => ModuleType.EnergyRetranslator;
}
