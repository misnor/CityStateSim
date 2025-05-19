namespace CityStateSim.Core.Components;
public struct StockpileComponent
{
    public Dictionary<string, int> Inventory { get; set; }
    public int CapacityPerResource { get; set; }
}
