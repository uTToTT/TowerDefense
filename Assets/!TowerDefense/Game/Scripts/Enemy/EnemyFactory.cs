using TToTT.TowerDefense.Enemies;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyFactory", menuName = "TD/Enemy/Enemy Factory")]
public class EnemyFactory : FactoryBase<Enemy>
{
    public EnemyType EnemyType => Prefab.EnemyType;
}
