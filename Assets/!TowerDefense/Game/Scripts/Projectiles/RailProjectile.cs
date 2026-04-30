using TToTT.TowerDefense.Enemies;
using UnityEngine;

public class RailProjectile : Projectile
{
    [SerializeField] private RailProjetileType _projectileType;

    private float _armorBreak;
    private float _critChance;
    private float _critMultiplier;

    public void SetCritMultiplier(float criticalMultiplier)
    {
        _critMultiplier = criticalMultiplier;
    }

    public void SetCritChance(float chance)
    {
        _critChance = chance;
    }

    public void SetArmorBreak(float armorBreak)
    {
        _armorBreak = armorBreak;
    }

    public void SetRailProjectileType(RailProjetileType type)
    {
        _projectileType = type;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (_projectileType == RailProjetileType.Default)
            {
                enemy.TakeDamage(_damage, _armorPiercing);
            }
            else if (_projectileType == RailProjetileType.ArmorBreak)
            {
                //Debug.Log("_armorBreak: " + _armorBreak);
                //enemy.TakeDamageToArmor(_armorBreak);
                enemy.TakeDamage(_damage, _armorPiercing);
            }
            else if (_projectileType == RailProjetileType.Crit)
            {
                if (Random.Range(0, 1) <= _critChance)
                {
                    float tmp = _damage;
                    tmp *= _critMultiplier;
                    enemy.TakeDamage((int)tmp, _armorPiercing);
                }
                else
                {
                    enemy.TakeDamage(_damage, _armorPiercing);
                }
            }

            DestroyProjectile();
        }
    }
}
