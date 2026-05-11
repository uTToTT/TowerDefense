using NaughtyAttributes;
using UnityEngine;

namespace TToTT.TowerDefense.Enemies.Wave
{
    [System.Serializable]
    public class Group
    {
        [HorizontalLine(color: EColor.Red)]
        [SerializeField] private RouteId _route;
        [SerializeField] private PathLane _lane;

        [HorizontalLine]
        [SerializeField] private EnemyType _enemyType;
        [SerializeField, Range(0, 200)] private int _enemyCount;
        [SerializeField, Range(0.1f, 10)] private float _timeBtwSpawn = 1;

        [HorizontalLine]
        [SerializeField, Range(-500, 500)] private float _hpAdditionalPercent = 0;
        [SerializeField, Range(-500, 500)] private float _moneyDropAdditionalPercent = 0;

        public RouteId RouteId => _route;
        public PathLane Lane => _lane;
        public EnemyType EnemyType => _enemyType;
        public int EnemyCount => _enemyCount;
        public float TimeBtwSpawn => _timeBtwSpawn;
        public float HpAdditionalPercent => _hpAdditionalPercent;
        public float MoneyDropAdditionalPercent => _moneyDropAdditionalPercent;
    }
}