using UnityEngine;

public class Gear : MapObject
{
    [SerializeField] private bool _isMotor;

    public int Power { get; private set; } = 1;
    public bool IsConnectedToMotor { get; set; }
    public bool IsMotor => _isMotor;
    public GearNetwork GearNetwork { get; set; }
}
