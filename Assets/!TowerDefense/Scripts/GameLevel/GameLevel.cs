using UnityEngine;

[CreateAssetMenu(fileName = "GameLevel_", menuName = "TD/Game level")]
public class GameLevel : ScriptableObject
{
    [SerializeField] public MapData mapData;

}
