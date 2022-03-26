using Sharp.Client.Client;
using Sharp.Data.Contexts;
using Sharp.Player.Manager;

namespace Sharp.Player.Services;

public class GameRegistrationService : BackgroundService
{
    private readonly IGameClient _gameClient;
    private readonly IGameManager _gameManager;
    private readonly ILogger<GameRegistrationService> _logger;
    private readonly IPlayerManager _playerManager;

    public GameRegistrationService(IGameClient gameClient, ILogger<GameRegistrationService> logger,IPlayerManager playerManager, IGameManager gameManager)
    {
        _gameClient = gameClient;
        _logger = logger;
        _playerManager = playerManager;
        _gameManager = gameManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Started {Service}", nameof(GameRegistrationService));
        var playerDetails = _playerManager.Get();

        var games = await _gameManager.GetAvailableGames();
        _logger.LogInformation("Retreived {N} open games to register", games.Count);
        await Task.WhenAll(games.Select(game => _gameManager.PerformRegistration(game.Id, playerDetails)));
    }
}