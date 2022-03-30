using Sharp.Client.Client;
using Sharp.Data.Contexts;
using Sharp.Player.Manager;

namespace Sharp.Player.Services;

public class GameRegistrationService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<GameRegistrationService> _logger;

    public GameRegistrationService(ILogger<GameRegistrationService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var gameManager = scope.ServiceProvider.GetRequiredService<IGameManager>();
        
        _logger.LogDebug("Started {Service}", nameof(GameRegistrationService));
        var games = await gameManager.GetAvailableGames();
        _logger.LogInformation("Retreived {N} open games to register", games.Count);
        await Task.WhenAll(games.Select(game => gameManager.PerformRegistration(game.Id)));
    }
}