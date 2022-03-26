using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Sharp.Player.Events.Models.Game;

public class RoundStatusEvent
{
    [JsonPropertyName("gameId")]
    [Required]
    public string GameId { get; set; }
    
    [JsonPropertyName("roundId")]
    [Required]
    public string RoundId { get; set; }
    
    [JsonPropertyName("roundNumber")]
    [Required]
    public uint RoundNumber { get; set; }
    
    [JsonPropertyName("roundStatus")]
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Required]
    public RoundStatus RoundStatus { get; set; }
}

public enum RoundStatus
{
    [EnumMember(Value = "started")]
    Started, 
    // Doesn't work because of the spaces. Shouldn't be any problem for now but sucks anyway
    [EnumMember(Value = "command input ended")]
    CommandInputEnded,
    
    [JsonPropertyName("ended")]
    Ended
} 