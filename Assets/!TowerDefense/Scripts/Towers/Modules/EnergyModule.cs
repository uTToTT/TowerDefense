using UnityEngine;

public sealed class EnergyModule : ITowerModule
{
    private Tower _tower;
    private EnergyModuleConfig _config;

    public ModuleType ModuleType => ModuleType.Energy;
    private float _receivedEnergy;

    public EnergyModule(EnergyModuleConfig config, Tower tower)
    {
        _config = config;
        _tower = tower;
    }

    public float GetConsumption() => _config.EnergyConsumpition;
    public float GetProduction() => _config.EnergyProduction;
    public float GetEnergyEffectivity() => _receivedEnergy / GetConsumption();
    public float SetReceivedEnergy(float amount) => _receivedEnergy = amount;

    public void Tick(float deltaTime)
    {
        if (_config != null)
            Debug.Log($"Curr energy effectivity [{GetEnergyEffectivity()}]");
    }

    public bool TryApplyConfig(TowerModuleConfig config)
    {
        if (config is EnergyModuleConfig energyConfig)
        {
            _config = energyConfig;
            return true;
        }

        return false;
    }
}
