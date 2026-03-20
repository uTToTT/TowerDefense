using System;
using UnityEngine;

public class CustomParticleSystem : MonoBehaviour, IPoolable, IEntityLifecycle
{
    public event Action<CustomParticleSystem> OnCompleted;

    [SerializeField] private ParticlesType _particleType;
    [SerializeField] private ParticleSystem _particleSystem;

    public ParticlesType Type => _particleType;

    public void Play() { _particleSystem.Play(); }

    public bool IsActive { get; set; }

    public void Dispose() { }
    public void OnActivated() { }
    public void OnDeactivated() { }
    public void OnDestroyed() { }
    public void OnPreload() { }
    public void OnReturned() { }

    private void OnParticleSystemStopped()
    {
        OnCompleted?.Invoke(this);
    }
}
