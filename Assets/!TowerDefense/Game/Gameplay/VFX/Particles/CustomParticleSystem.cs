using System;
using UnityEngine;

public class CustomParticleSystem : MonoBehaviour, IPoolable, IEntityLifecycle
{
    public event Action<CustomParticleSystem> OnCompleted;

    [SerializeField] private ParticlesType _particleType;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Vector3 _baseRotation;

    public ParticlesType Type => _particleType;
    public bool IsActive { get; set; }

    public void Apply(ParticleRequest request)
    {
        transform.position = request.Position;
        transform.rotation = Quaternion.Euler(
            request.Rotation + _baseRotation.x,
            0f + _baseRotation.y,
            0f + _baseRotation.z);
        transform.localScale = Vector3.one * request.Scale;
    }

    public void Play() => _particleSystem.Play();

    private void OnParticleSystemStopped() => OnCompleted?.Invoke(this);
    private void Reset()
    {
        if (_particleSystem == null)
            _particleSystem = GetComponent<ParticleSystem>();
    }
    public void OnPreload() { }
    public void OnActivated() { }
    public void OnDeactivated() { }

    public void OnReturned()
    {
        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void OnDestroyed() { }
    public void Dispose() { }
}