using NaughtyAttributes;
using UnityEngine;

[System.Serializable]
public class Group
{
    [HorizontalLine(color: EColor.Red)]
    [SerializeField] private RouteId _route;
    [SerializeField] private PathLane _lane;

    [HorizontalLine]
    [SerializeField] private EnemyType _enemyType;
    [SerializeField, Min(0)] private int _enemyCount;
    [SerializeField, Range(0.1f, 10)] private float _timeBtwSpawn = 1;

    [HorizontalLine]
    [SerializeField, Min(0.1f)] private float _hpMultiplier = 1;
    [SerializeField, Min(1)] private float _moneyDropMultiplier = 1;

    public RouteId RouteId => _route;
    public PathLane Lane => _lane;
    public EnemyType EnemyType => _enemyType;
    public int EnemyCount => _enemyCount;
    public float TimeBtwSpawn => _timeBtwSpawn;
    public float HpMultiplier => _hpMultiplier;
    public float MoneyDropMultiplier => _moneyDropMultiplier;
}
