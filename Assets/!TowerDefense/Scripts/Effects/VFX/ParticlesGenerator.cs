using UnityEngine;

public class ParticlesGenerator : MonoBehaviour
{
    [SerializeField] private ParticlesFactoryRegistry _factory;

    public static ParticlesGenerator Instance { get; private set; }

    #region Init

    public void Init()
    {
        Instance = this;
        _factory.Init();
    }

    public void Restart()
    {
        Dispose();
    }

    public void Dispose()
    {
        _factory.ReturnAll();
    }

    #endregion

    public void PlayParticles(ParticlesType type, Vector2 pos)
    {
        var cusPS = _factory.Create(type);
        cusPS.transform.position = pos;
        cusPS.OnCompleted += ReturnParticles;
        cusPS.Play();
    }

    private void ReturnParticles(CustomParticleSystem cusPS)
    {
        cusPS.OnCompleted -= ReturnParticles;
        _factory.Return(cusPS);
    }
}
