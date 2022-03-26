using Refit;
using Sharp.Client.Model;

namespace Sharp.Client.Client;

/// <summary>
///     Performs Player Registration.
/// </summary>
public interface IPlayerRegistrationClient
{
    [Post("/players")]
    Task<PlayerResponse> CreatePlayer([Body] PlayerRequest request);

    [Get("/players")]
    Task<PlayerResponse> GetPlayerDetails(string name, [AliasAs("mail")] string email);
}