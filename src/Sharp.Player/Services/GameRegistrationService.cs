using System.Net;
using Refit;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Data.Context;
using Sharp.Data.Model;
using Sharp.Player.Manager;

namespace Sharp.Player.Services;

public class GameRegistrationService : BackgroundService
{
    private readonly IGameClient _gameClient;
    private readonly ILogger<GameRegistrationService> _logger;
    private readonly IPlayerManager _playerManager;
    private readonly SharpDbContext _sharpDbContext;
    private readonly IGameManager _gameManager;

    public GameRegistrationService(IGameClient gameClient, ILogger<GameRegistrationService> logger,
        SharpDbContext sharpDbContext, IPlayerManager playerManager, IGameManager gameManager)
    {
        _gameClient = gameClient;
        _logger = logger;
        _sharpDbContext = sharpDbContext;
        _playerManager = playerManager;
        _gameManager = gameManager;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var playerDetails = _playerManager.Get();

        return GetGameIds(playerDetails.PlayerId).ContinueWith(games =>
        {
            var result = games.Result;
            _logger.LogInformation("Retreived {N} open games to register", result.Count);
            return Task.WhenAll(result.Select(id => _gameManager.PerformRegistration(id, playerDetails)));
        });
    }

    private async Task<List<string>> GetGameIds(string? playerId)
    {
        Task<List<GameResponse>> gamesToRegister;
        if (playerId == null)
            gamesToRegister = _gameClient.GetAllGamesOpenForRegistration();
        else
            gamesToRegister =
                _gameClient.GetAllGamesOpenForRegistrationAndPlayerNotYetRegistered(playerId);

        return (await gamesToRegister).Select(game => game.GameId).ToList();
    }
}