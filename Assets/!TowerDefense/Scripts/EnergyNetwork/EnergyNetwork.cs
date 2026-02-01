using System.Collections.Generic;

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
    }

    public bool HasEnoughEnergy =>
        TotalProduction >= TotalConsumption;
}
