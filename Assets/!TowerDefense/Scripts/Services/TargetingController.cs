using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class TargetingController : MonoBehaviour
{
    private List<Enemy> _targets;

    private void Reset()
    {
        var collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;
    }

    public Enemy GetNearestTargetToTower()
    {
        Enemy nearest = null;
        float minSqrDistance = float.MaxValue;
        Vector3 towerPos = transform.position;

        for (int i = _targets.Count - 1; i >= 0; i--)
        {
            var enemy = _targets[i];

            if (enemy == null || !enemy.IsAlive)
            {
                _targets.RemoveAt(i);
                continue;
            }

            float sqrDist = (enemy.transform.position - towerPos).sqrMagnitude;

            if (sqrDist < minSqrDistance)
            {
                minSqrDistance = sqrDist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    public Enemy GetNearestTargetToFinish()
    {
        Enemy nearest = null;
        float minRemainingDistance = float.MaxValue;

        for (int i = _targets.Count - 1; i >= 0; i--)
        {
            var enemy = _targets[i];

            if (enemy == null || !enemy.IsAlive)
            {
                _targets.RemoveAt(i);
                continue;
            }

            float remaining = enemy.RemainingDistance;

            if (remaining < minRemainingDistance)
            {
                minRemainingDistance = remaining;
                nearest = enemy;
            }
        }

        return nearest;
    }

    public Enemy GetTankiestTarget()
    {
        Enemy tankiest = null;
        float maxHp = float.MinValue;

        for (int i = _targets.Count - 1; i >= 0; i--)
        {
            var enemy = _targets[i];

            if (enemy == null || !enemy.IsAlive)
            {
                _targets.RemoveAt(i);
                continue;
            }

            float hp = enemy.MaxHp;

            if (hp > maxHp)
            {
                maxHp = hp;
                tankiest = enemy;
            }
        }

        return tankiest;
    }

    public Enemy GetSpeedestTarget()
    {
        Enemy fastest = null;
        float maxSpeed = float.MinValue;

        for (int i = _targets.Count - 1; i >= 0; i--)
        {
            var enemy = _targets[i];

            if (enemy == null || !enemy.IsAlive)
            {
                _targets.RemoveAt(i);
                continue;
            }

            float speed = enemy.CurrSpeed;

            if (speed > maxSpeed)
            {
                maxSpeed = speed;
                fastest = enemy;
            }
        }

        return fastest;
    }

    public Enemy GetMostArmoredTarget()
    {
        Enemy armored = null;
        float maxArmor = float.MinValue;

        for (int i = _targets.Count - 1; i >= 0; i--)
        {
            var enemy = _targets[i];

            if (enemy == null || !enemy.IsAlive)
            {
                _targets.RemoveAt(i);
                continue;
            }

            float armor = enemy.CurrArmor;

            if (armor > maxArmor)
            {
                maxArmor = armor;
                armored = enemy;
            }
        }

        return armored;
    }

    private void OnTriggerEnter2D(Collider2D collision) => Register(collision);
    private void OnTriggerExit2D(Collider2D collision) => Unregister(collision);

    private void Register(Collider2D collision)
    {
        if (collision.CompareTag(Tags.ENEMY))
        {
            if (collision.TryGetComponent<Enemy>(out var enemy))
                _targets.Add(enemy);
        }
    }

    private void Unregister(Collider2D collision)
    {
        if (collision.CompareTag(Tags.ENEMY))
        {
            if (collision.TryGetComponent<Enemy>(out var enemy))
                _targets.Remove(enemy);
        }
    }
}
