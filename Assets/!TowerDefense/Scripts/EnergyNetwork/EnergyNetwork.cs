using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class EnergyNetwork
{
    private readonly HashSet<IEnergyNode> _nodes = new();

    public IReadOnlyCollection<IEnergyNode> Nodes => _nodes;

    public float TotalProduction { get; private set; }
    public float TotalConsumption { get; private set; }

    public void AddNode(IEnergyNode node)
    {
        if (_nodes.Add(node))
        {
            Recalculate();
            node.EnergyNetwork = this;
        }
    }

    public void RemoveNode(IEnergyNode node)
    {
        if (_nodes.Remove(node))
        {
            Recalculate();
            node.EnergyNetwork = null;
        }
    }

    public void Recalculate()
    {
        TotalProduction = 0f;
        TotalConsumption = 0f;

        foreach (var node in _nodes)
        {
            TotalProduction += node.EnergyProduction;
            TotalConsumption += node.EnergyConsumption;
        }

        foreach (var node in _nodes)
            node.OnNetworkUpdated(this);

        DistributeEnergy();
    }

    public void DistributeEnergy()
    {
        var consumers = _nodes
            .Where(n => n.EnergyConsumption > 0f)
            .ToList();

        if (consumers.Count == 0)
            return;

        float totalConsumption = consumers.Sum(n => n.EnergyConsumption);
        float available = TotalProduction;

        foreach (var consumer in consumers)
        {
            float share = available * (consumer.EnergyConsumption / totalConsumption);
            consumer.SetReceivedEnergy(share);
        }
    }


    public bool HasEnoughEnergy =>
        TotalProduction >= TotalConsumption;

    private Color GetNetworkColor()
    {
        if (!HasEnoughEnergy)
            return Color.red;

        // стабильный псевдослучайный цвет по хэшу сети
        int hash = GetHashCode();
        Random.InitState(hash);

        return Color.HSVToRGB(
            Random.value,
            0.7f,
            0.9f
        );
    }

    public void DrawDebug()
    {
        if (_nodes.Count == 0)
            return;

        Gizmos.color = GetNetworkColor();

        foreach (var node in _nodes)
        {
            DrawNode(node);
        }

        DrawNodeConnections();
    }


    private void DrawNode(IEnergyNode node)
    {
        Vector3 pos = node.Transform.position;

        float radius = 0.18f;

        if (node.EnergyProduction > 0f)
            Gizmos.color = Color.cyan;
        else if (node.EnergyConsumption > 0f)
            Gizmos.color = Color.yellow;
        else
            Gizmos.color = Color.white;

        Gizmos.DrawSphere(pos, radius);
    }


    private void DrawNodeConnections()
    {
        var nodes = _nodes.ToList();

        Gizmos.color = GetNetworkColor();

        for (int i = 0; i < nodes.Count; i++)
        {
            var a = nodes[i];
            var aPorts = MapManager.GetWorldPorts(a);

            for (int j = i + 1; j < nodes.Count; j++)
            {
                var b = nodes[j];

                if (!EnergyNetworkManager.Instance.AreNodesConnected(aPorts, b))
                    continue;

                Gizmos.DrawLine(
                    a.Transform.position,
                    b.Transform.position
                );
            }
        }
    }

}
