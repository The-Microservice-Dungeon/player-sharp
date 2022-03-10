using System.Text.Json.Serialization;

namespace Player.Sharp.Core;

// TODO: Fancy Builder Pattern
public class Command
{
    [JsonPropertyName("gameId")] public string GameId { get; set; }

    [JsonPropertyName("playerToken")] public string PlayerToken { get; set; }

    [JsonPropertyName("robotId")] public string RobotId { get; set; }

    [JsonPropertyName("commandType")] public string CommandType { get; set; }

    [JsonPropertyName("commandObject")] public CommandObject CommandObject { get; set; } = new();
}

public class CommandObject
{
    [JsonPropertyName("commandType")] public string CommandType { get; set; }

    [JsonPropertyName("planetId")] public string PlanetId { get; set; }

    [JsonPropertyName("targetId")] public string TargetId { get; set; }

    [JsonPropertyName("itemName")] public string ItemName { get; set; }

    [JsonPropertyName("itemQuantity")] public uint ItemQty { get; set; }
}