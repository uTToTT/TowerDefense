using UnityEngine;

[CreateAssetMenu]
public class WavesInfo : ScriptableObject
{
    [SerializeField] private Wave[] _waves = { new Wave() };

    public Wave[] Waves => _waves;
}
