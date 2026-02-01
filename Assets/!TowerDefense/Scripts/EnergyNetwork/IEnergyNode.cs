public interface IEnergyNode : IMapObject
{
    EnergyNetwork EnergyNetwork { get; set; }

    float EnergyProduction { get;  }
    float EnergyConsumption { get; }

    void OnNetworkUpdated(EnergyNetwork network);
}
