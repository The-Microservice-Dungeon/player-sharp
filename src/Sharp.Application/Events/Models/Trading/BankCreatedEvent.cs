using System.Text.Json.Serialization;

namespace Sharp.Player.Events.Models.Trading;

public class BankCreatedEvent
{
    [JsonPropertyName("playerId")]
    public string PlayerId { get; set; }
    
    [JsonPropertyName("money")]
    public int Money { get; set; }
}