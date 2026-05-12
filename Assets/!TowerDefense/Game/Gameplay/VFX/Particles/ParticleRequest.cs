using UnityEngine;

public readonly struct ParticleRequest
{
    public readonly ParticlesType Type;
    public readonly Vector2 Position;
    public readonly float Rotation;   
    public readonly float Scale;      

    public ParticleRequest(
        ParticlesType type,
        Vector2 position,
        float rotation = 0f,
        float scale = 1f)
    {
        Type = type;
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public static ParticleRequest At(ParticlesType type, Vector2 pos)
        => new(type, pos);

    public static ParticleRequest WithDirection(
        ParticlesType type,
        Vector2 pos,
        Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return new(type, pos, angle);
    }
}