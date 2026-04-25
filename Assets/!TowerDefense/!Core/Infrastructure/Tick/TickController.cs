using System.Collections.Generic;

public class TickController
{
    private readonly List<ITickable> _systems = new();

    public void Register(ITickable system) => _systems.Add(system);
    public void Unregister(ITickable system) => _systems.Remove(system);

    public void Tick(float dt)
    {
        foreach (var system in _systems)
            system.Tick(dt);
    }
}