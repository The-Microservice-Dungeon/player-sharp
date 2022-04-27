using Refit;
using Sharp.Infrastructure.Network.Model;

namespace Sharp.Infrastructure.Network.Client;

/// <summary>
///     Client which interacts with games offered by the game service.
/// </summary>
public interface IGameClient
{
    [Put("/games/{gameId}/players/{playerToken}")]
    Task<GameRegistrationResponse> RegisterInGame(string gameId, string playerToken);

    [Head("/games/{gameId}")]
    Task<IApiResponse> CheckIfGameExists(string gameId);

    [Get("/games/{gameId}")]
    Task<GameResponse> GetGame(string gameId);

    [Get("/games")]
    Task<List<GameResponse>> GetAllActiveGames();

    async Task<List<GameResponse>> GetAllGamesOpenForRegistration()
    {
        return (await GetAllActiveGames())
            .Where(game => game.GameStatus == GameStatus.Created)
            .ToList();
    }

    // Nice method name
    async Task<List<GameResponse>> GetAllGamesOpenForRegistrationAndPlayerNotYetRegistered(string playerId)
    {
        return (await GetAllGamesOpenForRegistration())
            .Where(game => !game.ParticipatingPlayers.Contains(playerId))
            .ToList();
    }
}