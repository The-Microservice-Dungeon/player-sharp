using System.Text.Json.Serialization;

namespace Sharp.Client.Model;

public class PlayerResponse
{
    [JsonPropertyName("bearerToken")]
    public string BearerToken { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
}

public class PlayerRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }

    public PlayerRequest(string name, string email)
    {
        Name = name;
        Email = email;
    }
}