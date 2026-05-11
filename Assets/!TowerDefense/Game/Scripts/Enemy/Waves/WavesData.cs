using TToTT.TowerDefense.Enemies.Wave;
using UnityEngine;

[CreateAssetMenu(fileName = "WavesData_", menuName = "TD/Enemy/Waves Data")]
public class WavesData : ScriptableObject
{
    [SerializeField] private Wave[] _waves;

    public Wave[] Waves => _waves;
}
