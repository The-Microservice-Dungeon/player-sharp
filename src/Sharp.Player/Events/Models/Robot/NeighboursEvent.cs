using System.Text.Json.Serialization;

namespace Sharp.Player.Events.Models.Robot;

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