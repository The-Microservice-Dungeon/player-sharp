using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Sharp.Client.Model;

public class GameRegistrationResponse
{
    [JsonPropertyName("transactionId")] public string TransactionId { get; set; }
}

public enum GameStatus
{
    [EnumMember(Value = "created")] Created,
    [EnumMember(Value = "started")] Started,

    [EnumMember(Value = "ended")]
    Ended
}

public class GameResponse
{
    [JsonPropertyName("gameId")]
    [Required]
    public string GameId { get; set; }

    [JsonPropertyName("gameStatus")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Required]
    public GameStatus GameStatus { get; set; }

    [JsonPropertyName("participatingPlayers")]
    public List<string> ParticipatingPlayers { get; set; } = new();
}