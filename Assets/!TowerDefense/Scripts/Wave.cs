using UnityEngine;

[System.Serializable]
public class Wave 
{
    [SerializeField] private TypeEnemy _typeofEnemy;
    [SerializeField, Min(0)] private int _enemyCount;
    [SerializeField, Min(0)] private float _timeBtwSpawn;
    [SerializeField, Min(0.1f)] private float _hpMultiplier;
    [SerializeField, Min(1)] private float _moneyDropMultiplier;

    public TypeEnemy TypeOfEnemy => _typeofEnemy;
    public int EnemyCount => _enemyCount;
    public float TimeBtwSpawn => _timeBtwSpawn;
    public float HpMultiplier => _hpMultiplier;
    public float MoneyDropMultiplier => _moneyDropMultiplier;
}
