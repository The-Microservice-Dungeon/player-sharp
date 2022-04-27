using System.Text.Json.Serialization;

namespace Sharp.Player.Events.Models.Map;

public class SpacestationCreatedEvent
{
    [JsonPropertyName("planet_id")] public string PlanetId { get; set; }
}