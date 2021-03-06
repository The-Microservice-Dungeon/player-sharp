using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sharp.Infrastructure.Network.Model;

public class CommandRequest
{
    [JsonPropertyName("gameId")] public string GameId { get; set; }
    [JsonPropertyName("robotId")] public string RobotId { get; set; }

    [JsonPropertyName("playerToken")] public string PlayerToken { get; set; }

    [JsonPropertyName("commandType")] public string CommandType { get; set; }

    [JsonPropertyName("commandObject")] public CommandObjectRequest CommandObject { get; set; }
}

public class CommandObjectRequest
{
    [JsonPropertyName("commandType")] public string CommandType { get; set; }

    [JsonPropertyName("planetId")] public string? PlanetId { get; set; }

    [JsonPropertyName("targetId")] public string? TargetId { get; set; }

    [JsonPropertyName("itemName")] public string? ItemName { get; set; }

    [JsonPropertyName("itemQuantity")] public uint? ItemQuantity { get; set; }
}

public class ComamndResponse
{
    [JsonPropertyName("transactionId")]
    [Required]
    public string TransactionId { get; set; }
}