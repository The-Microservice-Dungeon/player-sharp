using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sharp.Gameplay.Map;

namespace Sharp.Player.Consumers.Model;

public class MovementEvent
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; }
    
    [JsonPropertyName("planet")]
    public MovementPlanet? Planet { get; set; }
    
    [JsonPropertyName("robots")]
    public string[]? Robots { get; set; }
}

public enum PlanetType
{
    [EnumMember(Value = "DEFAULT")]
    DEFAULT, 
    [EnumMember(Value = "SPACESTATION")]
    SPACESTATION
}

public class MovementPlanet
{
    [JsonPropertyName("planetId")]
    public string PlanetId { get; set; }
    
    [JsonPropertyName("movementDifficulty")]
    public int MovementDifficulty { get; set; }
    
    [JsonPropertyName("planetType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PlanetType PlanetType { get; set; }
    
    [JsonPropertyName("resourceType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ResourceType ResourceType { get; set; }
}