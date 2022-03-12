using Refit;
using Sharp.Client.Model;

namespace Sharp.Client.Client;

public interface IPlayerRegistrationClient
{
    [Post("/players")]
    Task<PlayerResponse> CreatePlayer([Body] PlayerRequest request);

    [Get("/players")]
    Task<PlayerResponse> GetPlayerDetails(string name, [AliasAs("mail")] string email);
}