using UnityEngine;

public class ParticlesGenerator
{
    private readonly ParticlesFactoryRegistry _factory;

    #region Init

    public ParticlesGenerator(ParticlesFactoryRegistry factory)
    {
        _factory = factory;
        _factory.Init();
    }

    public void Dispose()
    {
        _factory.ReturnAll();
    }

    #endregion

    #region Game loop

    public void Restart()
    {
        _factory.ReturnAll();
    }

    #endregion

    public void PlayParticles(ParticlesType type, Vector2 pos)
    {
        var ps = _factory.Create(type);
        ps.transform.position = pos;
        ps.OnCompleted += ReturnParticles;
        ps.Play();
    }

    private void ReturnParticles(CustomParticleSystem ps)
    {
        ps.OnCompleted -= ReturnParticles;
        _factory.Return(ps);
    }
}
