using UnityEngine;

[CreateAssetMenu]
public class WavesData : ScriptableObject
{
    [SerializeField] private Wave[] _waves;

    public Wave[] Waves => _waves;
}
