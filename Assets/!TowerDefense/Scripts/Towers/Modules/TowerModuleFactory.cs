using System;

public static class TowerModuleFactory
{
    public static ITowerModule Create(
        ITowerModule module,
        Tower tower)
    {
        return module switch
        {
            AttackModuleConfig attack => new AttackModule(attack, tower),

            _ => throw new ArgumentOutOfRangeException
            ($"Unknown module [{module.GetType()}]")
        };
    }
}
