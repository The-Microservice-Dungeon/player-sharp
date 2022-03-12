using System.Net;
using AutoMapper;
using Refit;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Core;
using Sharp.Data.Context;
using Sharp.Data.Model;

namespace Sharp.Player.Manager;

public class GameManager : IGameManager
{
    private readonly IGameClient _gameClient;
    private readonly SharpDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GameManager> _logger;

    public GameManager(IGameClient gameClient, SharpDbContext dbContext, IMapper mapper, ILogger<GameManager> logger)
    {
        _gameClient = gameClient;
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task PerformRegistration(string gameId, PlayerDetails playerDetails)
    {
        try
        {
            _logger.LogDebug("Registering for Game {GameId}", gameId);
            var game = await _gameClient.GetGame(gameId);
            if (game.GameStatus == GameStatus.Created)
            {
                if (playerDetails.PlayerId != null && game.ParticipatingPlayers.Contains(playerDetails.PlayerId))
                {
                    _logger.LogDebug("Player is already participating in the game");
                    return;
                }

                var transactionId = (await _gameClient.RegisterInGame(gameId, playerDetails.Token)).TransactionId;
                _logger.LogDebug("Successfully registered for game {Id}. The received TransactionId is {TransactionId}", gameId, transactionId);
                var registration = new GameRegistration(gameId, transactionId)
                {
                    PlayerDetails = playerDetails
                };
                _dbContext.GameRegistrations.Add(registration);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogDebug("The Game {GameId} is not open for registrations. State: {State}", gameId, game.GameStatus);
            }
        }
        catch (ApiException e)
        {
            if (e.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Could not register to the game {GameId}. A {Code} response was received. This might be caused by a dangling game or a duplicate registration", gameId, e.StatusCode);
                return;
            }
            throw;
        }
    }

    public async Task<List<Game>> GetAvailableGames()
    {
        var result = await _gameClient.GetAllGamesOpenForRegistration();
        return result.Select(response => _mapper.Map<Game>(response))
            .ToList();
    }
}