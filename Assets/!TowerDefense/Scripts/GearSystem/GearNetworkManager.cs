using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GearNetworkManager : MonoBehaviour
{
    private readonly List<GearNetwork> _networks = new();

    public void Init()
    {

    }

    public void AddGear(Gear gear)
    {
        if (_networks.Count == 0)
        {
            CreateNetwork(gear);
        }

        var visitedNetworks = new List<GearNetwork>();

        foreach (var offset in GearUtils.Offsets)
        {
            var pos = gear.MapPos + offset;

            if (MapManager.Instance.HasMapObject(pos, MapObjectType.Gear, out var mapObject))
            {
                var neighbour = mapObject as Gear;
                if (neighbour.GearNetwork != null && !visitedNetworks.Contains(neighbour.GearNetwork))
                {
                    visitedNetworks.Add(neighbour.GearNetwork);
                }
            }
        }

        if (visitedNetworks.Count == 1)
        {
            visitedNetworks[0].Register(gear);
        }
        else if(visitedNetworks.Count > 1)
        {
            MergeNetworks(visitedNetworks);
        }
    }

    private GearNetwork CreateNetwork(Gear gear)
    {
        var network = new GearNetwork();
        network.Register(gear);
        _networks.Add(network);

        return network;
    }

    private void MergeNetworks(List< GearNetwork> networks)
    {
        if (networks.Count <= 1)
            return;

        var root = networks[0];

        for (int ni = 1; ni < networks.Count; ni++)
        {
            var gears = networks[ni].Gears;

            for (int gi = 0; gi < networks[ni].Gears.Count; gi++)
            {
                root.Register(gears.ElementAt(gi));
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_networks == null || MapManager.Instance == null)
            return;

        const float pointRadius = 0.2f;

        for (int n = 0; n < _networks.Count; n++)
        {
            var net = _networks[n];

            Gizmos.color = Color.HSVToRGB(
                Mathf.Repeat(n * 0.173f, 1f), 
                0.8f,
                1f
            );

            for (int i = 0; i < net.Gears.Count; i++)
            {
                var gear = net.Gears.ElementAt(i);

                Vector3 pos = MapUtils.GridToWorld(gear.MapPos, MapManager.Instance.Grid);

                Gizmos.DrawSphere(pos, pointRadius);
            }
        }
    }
}
