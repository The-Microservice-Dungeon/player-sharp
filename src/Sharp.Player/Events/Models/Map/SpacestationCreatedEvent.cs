using System.Text.Json.Serialization;

namespace Sharp.Player.Consumers.Model;

public class SpacestationCreatedEvent
{
    [JsonPropertyName("planet_id")]
    public string PlanetId { get; set; }
}