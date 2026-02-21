using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(CircleCollider2D))]
public class TargetingModule : MonoBehaviour, ITowerModule
{
    public event Action<Enemy> OnTargetEntry;
    public event Action<Enemy> OnTargetExit;

    [SerializeField] private bool _debug;
    [HorizontalLine]

    [SerializeField] private CircleCollider2D _collider;
    [HorizontalLine]

    [SerializeField] private TypeTargetByCharacteristic _targetCharacteristic;
    [SerializeField] private TypeTargetByDistance _targetDistance;

    public TargetingModuleConfig Config;

    private List<Enemy> _targets = new();
    private List<Enemy> _toAdd = new();
    private List<Enemy> _toRemove = new();

    public ModuleType ModuleType => ModuleType.Targeting;

    public List<Enemy> Targets => _targets;

    #region Unity API

    private void Reset()
    {
        _collider = GetComponent<CircleCollider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) => Register(collision);
    private void OnTriggerExit2D(Collider2D collision) => Unregister(collision);

    private void OnDrawGizmos()
    {
        if (!_debug || Config == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Config.MinRange);
        Gizmos.DrawSphere(transform.position, Config.MaxRange);
    }

    #endregion

    public void Tick(float deltaTime) { RebuildTargets(); }

    public bool TryApplyConfig(TowerModuleConfig config)
    {
        if (config is not TargetingModuleConfig targetingConfig)
            return false;

        Config = targetingConfig;
        _collider.radius = Config.MaxRange;

        return true;
    }

    public void SetTargetSortingTypes(
        TypeTargetByCharacteristic byCharacteristic,
        TypeTargetByDistance byDistance)
    {
        _targetCharacteristic = byCharacteristic;
        _targetDistance = byDistance;
    }

    public Enemy GetTarget()
    {
        Enemy best = null;

        float bestCharacteristic = float.MinValue;
        float bestDistance = float.MaxValue;

        Vector3 towerPos = transform.position;

        float minRangeSqr = Config.MinRange * Config.MinRange;
        float maxRangeSqr = Config.MaxRange * Config.MaxRange;

        for (int i = _targets.Count - 1; i >= 0; i--)
        {
            var enemy = _targets[i];

            if (enemy == null || !enemy.IsAlive)
            {
                _toRemove.Add(enemy);
                continue;
            }

            Vector3 delta = enemy.transform.position - towerPos;
            float sqrToTower = delta.sqrMagnitude;

            if (sqrToTower < minRangeSqr || sqrToTower > maxRangeSqr)
                continue;

            float characteristic = _targetCharacteristic switch
            {
                TypeTargetByCharacteristic.MaxHP => enemy.MaxHp,
                TypeTargetByCharacteristic.Speed => enemy.CurrSpeed,
                TypeTargetByCharacteristic.Armor => enemy.CurrArmor,
                _ => 0f
            };

            float distance = _targetDistance switch
            {
                TypeTargetByDistance.ToTower => sqrToTower,
                TypeTargetByDistance.ToExit => enemy.RemainingDistance,
                _ => 0f
            };

            bool isBetter =
                characteristic > bestCharacteristic ||
                (Mathf.Approximately(characteristic, bestCharacteristic) &&
                 distance < bestDistance);

            if (best == null || isBetter)
            {
                best = enemy;
                bestCharacteristic = characteristic;
                bestDistance = distance;
            }
        }

        return best;
    }

    private void RebuildTargets()
    {
        for (int i = 0; i < _toAdd.Count; i++)
        {
            _targets.Add(_toAdd[i]);
            OnTargetEntry?.Invoke(_toAdd[i]);
        }

        _toAdd.Clear();

        for (int i = 0; i < _toRemove.Count; i++)
        {
            _targets.Remove(_toRemove[i]);
            OnTargetExit?.Invoke(_toRemove[i]);
        }

        _toRemove.Clear();
    }

    private void Register(Collider2D collision)
    {
        if (collision.CompareTag(Tags.ENEMY))
        {
            if (collision.TryGetComponent<Enemy>(out var enemy))
            {
                _toAdd.Add(enemy);
                Debug.Log($"Register enemy in range [{gameObject.name}]");
            }
        }
    }

    private void Unregister(Collider2D collision)
    {
        if (collision.CompareTag(Tags.ENEMY))
        {
            if (collision.TryGetComponent<Enemy>(out var enemy))
            {
                _toRemove.Add(enemy);
            }
        }
    }
}
