using Sharp.Client.Client;
using Sharp.Data.Contexts;
using Sharp.Player.Manager;

namespace Sharp.Player.Services;

public class GameRegistrationService : BackgroundService
{
    private readonly IGameManager _gameManager;
    private readonly ILogger<GameRegistrationService> _logger;

    public GameRegistrationService(ILogger<GameRegistrationService> logger, IGameManager gameManager)
    {
        _logger = logger;
        _gameManager = gameManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Started {Service}", nameof(GameRegistrationService));
        var games = await _gameManager.GetAvailableGames();
        _logger.LogInformation("Retreived {N} open games to register", games.Count);
        await Task.WhenAll(games.Select(game => _gameManager.PerformRegistration(game.Id)));
    }
}