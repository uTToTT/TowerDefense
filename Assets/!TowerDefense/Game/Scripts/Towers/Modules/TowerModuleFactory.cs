using System;

public static class TowerModuleFactory
{
    public static ITowerModule Create(
        TowerModuleConfig config,
        Tower tower)
    {
        return config switch
        {
            AttackModuleConfig attack => new AttackModule(attack, tower),
            EnergyModuleConfig energy => new EnergyModule(energy, tower),
            SlowdownModuleConfig slow => new SlowdownModule(slow, tower),

            _ => throw new ArgumentOutOfRangeException
            ($"Unknown module [{config.GetType()}]")
        };
    }
}
