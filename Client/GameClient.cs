using System.Text.Json.Serialization;
using Player.Sharp.Core;
using Refit;

namespace Player.Sharp.Client;

public class RegistrationResponse
{
    [JsonPropertyName("transactionId")] public string TransactionId { get; set; }
}

public class PlayerResponse
{
    [JsonPropertyName("bearerToken")] public string BearerToken { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("email")] public string Email { get; set; }
}

public class PlayerRequest
{
    public PlayerRequest(string name, string email)
    {
        Name = name;
        Email = email;
    }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("email")] public string Email { get; set; }
}

public class CommandResponse
{
    [JsonPropertyName("transactionId")] public string TransactionId { get; set; }
}

public interface IGameClient
{
    [Put("/games/{gameId}/players/{playerToken}")]
    Task<RegistrationResponse> RegisterForGame(string gameId, string playerToken);

    [Post("/players")]
    Task<PlayerResponse> CreatePlayer([Body] PlayerRequest request);

    [Get("/players")]
    Task<PlayerResponse> GetPlayerDetails(string name, [AliasAs("mail")] string email);

    [Post("/commands")]
    Task<CommandResponse> PostCommand([Body] Command command);
}