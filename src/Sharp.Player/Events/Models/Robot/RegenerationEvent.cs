using System.Text.Json.Serialization;

namespace Sharp.Player.Events.Models.Robot;

public class RegenerationEvent
{
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }
    [JsonPropertyName("remainingEnergy")] public uint Energy { get; set; }
}