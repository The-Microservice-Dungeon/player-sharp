using System.Net;
using AutoMapper;
using Refit;
using Sharp.Domain.Game;
using Sharp.Infrastructure.Network.Client;
using Sharp.Infrastructure.Network.Model;
using Sharp.Infrastructure.Persistence.Contexts;
using Sharp.Infrastructure.Persistence.Models;
using Sharp.Player.Provider;

namespace Sharp.Player.Manager;

public class GameManager : IGameManager
{
    private readonly SharpDbContext _db;
    private readonly IGameClient _gameClient;
    private readonly ILogger<GameManager> _logger;
    private readonly IMapper _mapper;
    private readonly IPlayerDetailsProvider _playerDetailsProvider;

    public GameManager(IGameClient gameClient, IMapper mapper, ILogger<GameManager> logger, SharpDbContext db,
        IPlayerDetailsProvider playerDetailsProvider)
    {
        _gameClient = gameClient;
        _mapper = mapper;
        _logger = logger;
        _db = db;
        _playerDetailsProvider = playerDetailsProvider;
    }

    public async Task PerformRegistration(string gameId)
    {
        try
        {
            _logger.LogDebug("Registering for Game {GameId}", gameId);
            var game = await _gameClient.GetGame(gameId);
            if (game.GameStatus == GameStatus.Created)
            {
                var playerDetails = await _playerDetailsProvider.GetAsync();

                if (playerDetails.PlayerId != null && game.ParticipatingPlayers.Contains(playerDetails.PlayerId))
                {
                    _logger.LogDebug("Player is already participating in the game");
                    return;
                }

                var transactionId = (await _gameClient.RegisterInGame(gameId, playerDetails.Token)).TransactionId;
                _logger.LogDebug(
                    "Successfully registered for game {Id}. The received TransactionId is {TransactionId}",
                    gameId, transactionId);

                _db.PlayerDetails.Attach(playerDetails);
                var registration = new GameRegistration(gameId, transactionId);
                await _db.GameRegistrations.AddAsync(registration);
                await _db.SaveChangesAsync();
            }
            else
            {
                _logger.LogDebug("The Game {GameId} is not open for registrations. State: {State}", gameId,
                    game.GameStatus);
            }
        }
        catch (ApiException e)
        {
            if (e.StatusCode is not (HttpStatusCode.Forbidden or HttpStatusCode.NotFound)) throw;

            _logger.LogWarning(
                "Could not register to the game {GameId}. A {Code} response was received. This might be caused by a dangling game or a duplicate registration",
                gameId, e.StatusCode);
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
        return Task.Run(() => _db.GameRegistrations
            .Select(registration => _mapper.Map<Game>(registration))
            .ToList());
    }

    public GameRegistration? ResolveRegistration(string transactionId)
    {
        return _db.GameRegistrations
            .FirstOrDefault(registration => registration.TransactionId == transactionId);
    }
}