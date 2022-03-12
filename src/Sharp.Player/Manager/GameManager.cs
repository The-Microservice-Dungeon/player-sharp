using System.Net;
using Refit;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Data.Context;
using Sharp.Data.Model;

namespace Sharp.Player.Manager;

public class GameManager : IGameManager
{
    private readonly IGameClient _gameClient;
    private readonly SharpDbContext _dbContext;

    public GameManager(IGameClient gameClient, SharpDbContext dbContext)
    {
        _gameClient = gameClient;
        _dbContext = dbContext;
    }

    public async Task PerformRegistration(string gameId, PlayerDetails playerDetails)
    {
        try
        {
            var game = await _gameClient.GetGame(gameId);
            if (game.GameStatus == GameStatus.Created)
            {
                if (playerDetails.PlayerId != null && game.ParticipatingPlayers.Contains(playerDetails.PlayerId))
                    return;
                
                var result = await _gameClient.RegisterInGame(gameId, playerDetails.Token);
                var registration = new GameRegistration(gameId, result.TransactionId)
                {
                    PlayerDetails = playerDetails
                };
                _dbContext.GameRegistrations.Add(registration);
                await _dbContext.SaveChangesAsync();
            }
        }
        catch (ApiException e)
        {
            if (e.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.NotFound)
                return;
            throw;
        }
    }
}