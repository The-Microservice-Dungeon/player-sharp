using System.Text.Json.Serialization;

namespace Sharp.Player.Consumers.Models.Trading;

public class TradeSellResourcesEvent : TradeBaseEvent<TradeResourcesData>
{
    
}

public class TradeResourcesData
{
    [JsonPropertyName("coal")]
    public uint Coal { get; set; }
    
    [JsonPropertyName("iron")]
    public uint Iron { get; set; }
    
    [JsonPropertyName("gem")]
    public uint Gem { get; set; }
    
    [JsonPropertyName("gold")]
    public uint Gold { get; set; }
    
    [JsonPropertyName("platin")]
    public uint Platin { get; set; }
}