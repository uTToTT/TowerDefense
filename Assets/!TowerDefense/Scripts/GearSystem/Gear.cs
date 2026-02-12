using UnityEngine;

public class Gear : MonoBehaviour, IMapObject
{
    [SerializeField] private MapObjectShape _shape;

    public Transform Transform => transform;
    public Vector2Int MapPos { get; set; }
    public MapObjectShape Shape => _shape;

    public int Power { get; private set; } = 1;
    public bool IsConnectedToMotor { get; set; }
}
