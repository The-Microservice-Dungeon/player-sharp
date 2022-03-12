using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Sharp.Client.Model;

public class GameRegistrationResponse
{
    [JsonPropertyName("transactionId")]
    public string TransactionId { get; set; }
}

public enum GameStatus
{
    [EnumMember(Value = "CREATED")]
    Created, 
    [EnumMember(Value = "STARTED")]
    Started, 
    [EnumMember(Value = "COMMAND_INPUT_ENDED")]
    CommandInputEnded
}

public class GameResponse
{
    [JsonPropertyName("gameId")]
    public string GameId { get; set; }
    
    [JsonPropertyName("gameStatus")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GameStatus GameStatus { get; set; }
    
    [JsonPropertyName("maxPlayers")]
    public uint MaxPlayers { get; set; }
    
    [JsonPropertyName("maxRounds")]
    public uint MaxRounds { get; set; }
    
    [JsonPropertyName("currentRoundNumber")]
    public uint CurrentRoundNumbers { get; set; }
    
    [JsonPropertyName("roundLengthInMillis")]
    public uint RoundLengthInMillis { get; set; }
    
    [JsonPropertyName("participatingPlayers")]
    public string[] ParticipatingPlayers { get; set; }
}