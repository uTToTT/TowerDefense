using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ParticlesFactoryRegistry",
    menuName = "TD/Effects/VFX/Particles/Registry",
    order = 0)]
public class ParticlesFactoryRegistry : ScriptableObject
{
    [SerializeField] private ParticlesFactory[] _factories;

    private Dictionary<ParticlesType, ParticlesFactory> _map;

    public void Init()
    {
        _map = new Dictionary<ParticlesType, ParticlesFactory>();
        foreach (var factory in _factories)
        {
            factory.Init();
            _map.Add(factory.Type, factory);
        }
    }

    public CustomParticleSystem Create(ParticlesType type)
    {
        return _map[type].Create();
    }

    public void Return(CustomParticleSystem enemy)
    {
        _map[enemy.Type].Return(enemy);
    }

    public void ReturnAll()
    {
        foreach (var factory in _map.Values)
        {
            factory.ReturnAll();
        }
    }

    public void Dispose(CustomParticleSystem enemy)
    {
        _map[enemy.Type].Dispose();
    }
}
