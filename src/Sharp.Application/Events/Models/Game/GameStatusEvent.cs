using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Sharp.Infrastructure.Network.Model;

namespace Sharp.Player.Events.Models.Game;

public class GameStatusEvent
{
    [JsonPropertyName("gameId")]
    [Required]
    public string GameId { get; set; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Required]
    public GameStatus Status { get; set; }
}