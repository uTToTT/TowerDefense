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

            _ => throw new ArgumentOutOfRangeException
            ($"Unknown module [{config.GetType()}]")
        };
    }
}
