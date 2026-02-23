using UnityEngine;

[CreateAssetMenu(fileName = "ParticlesFactory", menuName = "TD/Effects/VFX/Particles/Factory")]
public class ParticlesFactory : FactoryBase<CustomParticleSystem>
{
    public  ParticlesType Type => Prefab.Type; 
}
