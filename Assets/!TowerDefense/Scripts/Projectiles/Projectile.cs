using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected TrailRenderer _trailRenderer;
    [SerializeField] protected LayerMask _enemyLayerMask;
    [Space]
    [SerializeField] private bool _trailDestroyed;
    [SerializeField] private bool _gameobjDestroyed;
    [SerializeField] private bool _gameobjDestroyed2;


    protected int _damage;
    protected float _armorPiercing;
    protected float _speedProjectile;
    private bool _destroyPending;
    public Enemy _target;

    public void DestroyProjectile()
    {
        if (gameObject != null)
        {
            if (_trailRenderer != null)
            {
                _trailRenderer.transform.parent = null;
                _trailDestroyed = true;
                Destroy(_trailRenderer.gameObject, 1f);
            }

            _gameobjDestroyed = true;
            //Destroy(gameObject);
            _destroyPending = true;
            _gameobjDestroyed2 = true;
        }
    }

    private void Update()
    {
        if (_destroyPending)
        {
            _destroyPending = false;
            Destroy(gameObject);
        }
    }

    public void SetProjectileSpeed(float speed)
    {
        _speedProjectile = speed;
    }

    public void SetArmorPiercing(float piercing)
    {
        _armorPiercing = piercing;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    protected Collider2D[] GetColidersInRadius(float radius)
    {
        return Physics2D.OverlapCircleAll(transform.position, radius, _enemyLayerMask);
    }
}

public enum MinigunProjectileType
{
    Default,
    Explosion,
    Freeze,
}

public enum TwiinProjectileType
{
    Default,
    Shard,
}

public enum RailProjetileType 
{
    Default,
    Crit,
    ArmorBreak,
}
