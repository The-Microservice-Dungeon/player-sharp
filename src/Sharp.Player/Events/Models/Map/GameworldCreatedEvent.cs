using System.Text.Json.Serialization;

namespace Sharp.Player.Events.Models.Map;

public class GameworldCreatedEvent
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("spacestation_ids")]
    public List<string> SpacestationIds { get; set; }
}