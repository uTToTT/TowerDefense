using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class EnergyNetworkManager : MonoBehaviour
{
    [ProgressBar(nameof(Energy), nameof(Consumption), EColor.Yellow)]
    public float Energy;

    private readonly List<EnergyNetwork> _networks = new();

    public static EnergyNetworkManager Instance { get; private set; }

    public float Consumption
    {
        get
        {
            float consumption = 0;
            foreach (EnergyNetwork network in _networks)
            {
                consumption += network.TotalConsumption;
            }
            return consumption;
        }
    }

    public void Init()
    {
        Instance = this;
    }

    public void RegisterNode(IEnergyNode node)
    {
        var connectedNetworks = FindConnectedNetworks(node);

        if (connectedNetworks.Count == 0)
        {
            var network = new EnergyNetwork();
            network.AddNode(node);
            _networks.Add(network);
        }
        else
        {
            var main = connectedNetworks[0];
            main.AddNode(node);

            for (int i = 1; i < connectedNetworks.Count; i++)
            {
                MergeNetworks(main, connectedNetworks[i]);
            }
        }

        float prod = 0;
        foreach (EnergyNetwork network in _networks)
        {
            prod += network.TotalProduction;
        }
        Energy = prod;
    }

    public void UnregisterNode(IEnergyNode node)
    {
        var network = FindNetworkContaining(node);
        if (network == null)
            return;

        network.RemoveNode(node);
        _networks.Remove(network);

        RebuildDisconnected(network.Nodes);
    }

    private EnergyNetwork FindNetworkContaining(IEnergyNode node) => node.EnergyNetwork;

    private void MergeNetworks(EnergyNetwork main, EnergyNetwork network)
    {
        foreach (var node in network.Nodes)
        {
            main.AddNode(node);
        }

        _networks.Remove(network);
    }

    private List<EnergyNetwork> FindConnectedNetworks(IEnergyNode node)
    {
        var result = new List<EnergyNetwork>();
        var nodePorts = MapManager.GetWorldPorts(node);

        foreach (var network in _networks)
        {
            foreach (var other in network.Nodes)
            {
                if (AreNodesConnected(nodePorts, other))
                {
                    result.Add(network);
                    break;
                }
            }
        }

        return result;
    }

    private bool AreNodesConnected(List<WorldPort> aPorts, IEnergyNode other)
    {
        var bPorts = MapManager.GetWorldPorts(other);

        foreach (var a in aPorts)
            foreach (var b in bPorts)
            {
                if (MapManager.Instance.ArePortsConnected(a, b))
                    return true;
            }

        return false;
    }

    private void RebuildDisconnected(IEnumerable<IEnergyNode> nodes)
    {
        var unvisited = new HashSet<IEnergyNode>(nodes);

        while (unvisited.Count > 0)
        {
            var start = unvisited.First();
            var group = FloodFill(start, unvisited);

            var network = new EnergyNetwork();

            foreach (var node in group)
            {
                network.AddNode(node);
                unvisited.Remove(node);
            }

            _networks.Add(network);
        }
    }

    private List<IEnergyNode> FloodFill(
    IEnergyNode start,
    HashSet<IEnergyNode> allowed)
    {
        var result = new List<IEnergyNode>();
        var stack = new Stack<IEnergyNode>();
        var visited = new HashSet<IEnergyNode>();

        stack.Push(start);
        visited.Add(start);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            result.Add(current);

            var currentPorts = MapManager.GetWorldPorts(current);

            foreach (var other in allowed)
            {
                if (visited.Contains(other))
                    continue;

                if (AreNodesConnected(currentPorts, other))
                {
                    visited.Add(other);
                    stack.Push(other);
                }
            }
        }

        return result;
    }
}