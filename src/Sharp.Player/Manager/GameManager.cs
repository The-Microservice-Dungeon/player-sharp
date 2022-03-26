using System.Net;
using AutoMapper;
using Refit;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Data.Contexts;
using Sharp.Data.Models;
using Sharp.Gameplay.Game;

namespace Sharp.Player.Manager;

public class GameManager : IGameManager
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IGameClient _gameClient;
    private readonly ILogger<GameManager> _logger;
    private readonly IMapper _mapper;

    public GameManager(IGameClient gameClient, IServiceScopeFactory scopeFactory, IMapper mapper, ILogger<GameManager> logger)
    {
        _gameClient = gameClient;
        _scopeFactory = scopeFactory;
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
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SharpDbContext>();
                if (playerDetails.PlayerId != null && game.ParticipatingPlayers.Contains(playerDetails.PlayerId))
                {
                    _logger.LogDebug("Player is already participating in the game");
                    return;
                }

                var transactionId = (await _gameClient.RegisterInGame(gameId, playerDetails.Token)).TransactionId;
                _logger.LogDebug(
                    "Successfully registered for game {Id}. The received TransactionId is {TransactionId}",
                    gameId, transactionId);
                var registration = new GameRegistration(gameId, transactionId)
                {
                    PlayerDetails = playerDetails
                };
                await db.GameRegistrations.AddAsync(registration);
                await db.SaveChangesAsync();
            }
            else
            {
                _logger.LogDebug("The Game {GameId} is not open for registrations. State: {State}", gameId,
                    game.GameStatus);
            }
        }
        catch (ApiException e)
        {
            if (e.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.NotFound)
            {
                _logger.LogWarning(
                    "Could not register to the game {GameId}. A {Code} response was received. This might be caused by a dangling game or a duplicate registration",
                    gameId, e.StatusCode);
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

    public Task<List<Game>> GetRegisteredGames()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SharpDbContext>();
        return Task.Run(() => db.GameRegistrations
            .Select(registration => _mapper.Map<Game>(registration))
            .ToList());
    }
}