using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Sharp.Client.Model;

namespace Sharp.Player.Consumers.Model;

public class GameStatusEvent
{
    [JsonPropertyName("gameId")]
    [Required]
    public string GameId { get; set; }
    
    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Required]
    public GameStatus Status { get; set; }
}