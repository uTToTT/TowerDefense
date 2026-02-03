using UnityEngine;

public class EnergyRetranslatorModule : ITowerModule
{
    private Tower _tower;
    private EnergyRetranslatorModuleConfig _config;

    public ModuleType ModuleType => ModuleType.EnergyRetranslator;

    public EnergyRetranslatorModule(EnergyRetranslatorModuleConfig config, Tower tower)
    {
        _config = config;
        _tower = tower;
    }

    public void Tick(float deltaTime) { }

    public bool TryApplyConfig(TowerModuleConfig config)
    {
        if (config is EnergyRetranslatorModuleConfig retConfig)
        {
            _config = retConfig;
            return true;
        }

        return false;
    }

    public bool IsInsideRetranslation(Vector2Int vector2Int)
    {
        if (_tower.MapPos.x - _config.TranslationRadius <= vector2Int.x &&
            _tower.MapPos.y - _config.TranslationRadius <= vector2Int.y &&
            _tower.MapPos.x + _config.TranslationRadius >= vector2Int.x &&
            _tower.MapPos.y + _config.TranslationRadius >= vector2Int.y)
        {
            return true;
        }

        return false;
    }
}
