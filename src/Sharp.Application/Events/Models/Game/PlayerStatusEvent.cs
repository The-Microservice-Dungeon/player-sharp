using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sharp.Player.Events.Models.Game;

public class PlayerStatusEvent
{
    [Required]
    [JsonPropertyName("playerId")]
    public string PlayerId { get; set; }

    [Required] [JsonPropertyName("name")] public string Name { get; set; }
}