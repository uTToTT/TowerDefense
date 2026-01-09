using UnityEngine;

[CreateAssetMenu(fileName = "GameLevel_", menuName = "TD/Game level")]
public class GameLevel : ScriptableObject
{
    [SerializeField] private MapData _mapData;
    [SerializeField] private WavesData _wavesData;

    public MapData MapData => _mapData;
    public WavesData WavesData => _wavesData;
}
