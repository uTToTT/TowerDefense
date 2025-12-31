using System.Collections;
using UnityEngine;

public class TwiinProjectile : Projectile
{
    [SerializeField] private TwiinProjectileType _projectileType;
    [SerializeField] private bool _isDestroyed;

    private int _shardDamage;
    private float _shardExplodeRadius;
    private int _maxNumShard;
    private float _speedShard;

    private bool _isShard;

    private void ExplodeShard()
    {
        Collider2D[] colliders = GetColidersInRadius(_shardExplodeRadius);

        if (colliders.Length != 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    if (_target != null && colliders[i].transform == _target?.transform)
                    {
                        continue;
                    }
                }

                if (i >= _maxNumShard)
                {
                    break;
                }

                TwiinProjectile projectile = Instantiate(this, transform.position, Quaternion.identity);
                projectile.SetIsShard();
                projectile.SetDamage(_shardDamage);
                projectile.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                projectile._target = colliders[i].GetComponent<Enemy>();
                projectile.StartMoveProjectileToEnemy(colliders[i].GetComponent<Enemy>(), _speedShard);
                Destroy(projectile.gameObject, 3f);
            }
        }
        else
        {
            DestroyProjectile();
        }
    }

    protected void StartMoveProjectileToEnemy(Enemy enemy, float projectileSpeed)
    {
        StartCoroutine(MoveProjectileToEnemy(enemy, projectileSpeed));
    }

    protected IEnumerator MoveProjectileToEnemy(Enemy enemy, float projectileSpeed)
    {
        while (enemy != null || _target != null)
        {
            var dir = enemy.transform.position - transform.position;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angleDirection - 90, Vector3.forward);
            transform.localPosition = Vector2.MoveTowards(transform.position, enemy.transform.position, projectileSpeed * Time.deltaTime);
            yield return null;
        }
        //Debug.Log("Enemy or target is null, destroying projectile");
        if (this != null && enemy == null || _target == null)
        {
            this.DestroyProjectile();
        }
    }

    public void SetShardSpeed(float speed)
    {
        _speedShard = speed;
    }

    public void SetShardDamage(int damage)
    {
        _shardDamage = damage;
    }

    public void SetMaxNumShard(int num)
    {
        _maxNumShard = num;
    }

    public void SetShardExplodeRadius(float radius)
    {
        _shardExplodeRadius = radius;
    }

    public void SetIsShard()
    {
        _isShard = true;
    }

    public void SetTwiinProjectileType(TwiinProjectileType type)
    {
        _projectileType = type;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            Enemy target = collision.GetComponent<Enemy>();

            target.TakeDamage(_damage, _armorPiercing);

            if (_projectileType == TwiinProjectileType.Default)
            {

            }
            else if (_projectileType == TwiinProjectileType.Shard)
            {
                if (!_isShard)
                {
                    ExplodeShard();
                }
            }

            _isDestroyed = true;
            DestroyProjectile();
        }
    }
}
