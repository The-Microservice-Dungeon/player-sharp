using System.Text.Json.Serialization;

namespace Sharp.Infrastructure.Network.Model;

public class PlayerResponse
{
    [JsonPropertyName("playerId")] public string PlayerId { get; set; }
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