using UnityEngine;

[CreateAssetMenu]
public class WavesInfo : ScriptableObject
{
    [SerializeField] private Wave[] _waves;

    public Wave[] Waves => _waves;
}
