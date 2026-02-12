using System.Collections.Generic;
using UnityEngine;

public class GearNetwork : MonoBehaviour
{
    private readonly HashSet<Gear> _gears = new();

    public int TotalPower { get; private set; }

    public void Register(Gear gear) => _gears.Add(gear);
    public void Unregister(Gear gear) => _gears.Remove(gear);

    public void Calculate()
    {
        TotalPower = 0;

        foreach (Gear gear in _gears)
        {
            TotalPower += gear.Power;
        }
    }
}
