using Refit;
using Sharp.Client.Model;

namespace Sharp.Client.Client;

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
}