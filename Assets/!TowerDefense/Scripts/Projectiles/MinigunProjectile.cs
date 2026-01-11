using UnityEngine;

public class MinigunProjectile : Projectile
{
    [SerializeField] private MinigunProjectileType _projectileType;
    [Space]
    [Header("Explosion")]
    [SerializeField] private ParticleSystem _explosionVFX;
    [Space]
    [Header("Freeze")]
    [SerializeField] private ParticleSystem _freezeVFX;

    private int _explosionDamage;
    private float _radiusExplosion;

    private int _freezeIncrement;
    private float _freezeRadius;

    private void Freeze(Enemy enemy)
    {
        Collider2D[] colliders = GetColidersInRadius(_freezeRadius);

        foreach (Collider2D collider in colliders)
        {
        }

        if (_freezeVFX != null)
        {
            Destroy(Instantiate(_freezeVFX, transform.position, Quaternion.identity).gameObject, 2f);
        }
    }

    private void Explode()
    {
        Collider2D[] colliders = GetColidersInRadius(_radiusExplosion);

        foreach (Collider2D collider in colliders)
        {
            collider.GetComponent<Enemy>().TakeDamage(_explosionDamage, _armorPiercing);
        }

        if (_explosionVFX != null)
        {
            Destroy(Instantiate(_explosionVFX, transform.position, Quaternion.identity).gameObject, 2f);
        }
    }

    public void SetExplosionDamage(int explosionDamage)
    {
        _explosionDamage = explosionDamage;
    }

    public void SetExplosionRadius(float radius)
    {
        _radiusExplosion = radius;
    }

    public void SetFreezeRadius(float radius)
    {
        _freezeRadius = radius;
    }

    public void SetFreezeIncrement(int freezeIncrement)
    {
        _freezeIncrement = freezeIncrement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            Enemy target = collision.GetComponent<Enemy>();

            target.TakeDamage(_damage, _armorPiercing);

            if (_projectileType == MinigunProjectileType.Explosion)
            {
                Explode();
            }
            else if (_projectileType == MinigunProjectileType.Freeze)
            {
                Freeze(target);
            }

            DestroyProjectile();
        }
    }
}
