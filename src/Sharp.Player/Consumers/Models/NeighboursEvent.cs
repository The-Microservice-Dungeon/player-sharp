using System.Text.Json.Serialization;

namespace Sharp.Player.Consumers.Model;

public class NeighboursEvent
{
    [JsonPropertyName("neighbours")]
    public NeighbourPlanet[] Neighbours { get; set; }
}

public class NeighbourPlanet
{
    [JsonPropertyName("planetId")]
    public String PlanetId { get; set; }
    [JsonPropertyName("movementDifficulty")]
    public int MovementDifficulty { get; set; }
}