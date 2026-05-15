using System;

public class ParticlesGenerator : IDisposable
{
    private readonly ParticlesFactoryRegistry _factory;

    public ParticlesGenerator(ParticlesFactoryRegistry factory)
    {
        _factory = factory;
        _factory.Init();
    }

    public void Play(ParticleRequest request)
    {
        var ps = _factory.Create(request.Type);
        ps.Apply(request);
        ps.OnCompleted += OnParticleCompleted;
        ps.Play();
    }

    private void OnParticleCompleted(CustomParticleSystem ps)
    {
        ps.OnCompleted -= OnParticleCompleted;
        _factory.Return(ps);
    }

    public void Restart() => _factory.ReturnAll();

    public void Dispose() => _factory.ReturnAll();
}