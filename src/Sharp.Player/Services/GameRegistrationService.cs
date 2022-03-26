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
    private readonly SharpDbContext _sharpDbContext;

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

        return _gameManager.GetAvailableGames().ContinueWith(games =>
        {
            var result = games.Result;
            _logger.LogInformation("Retreived {N} open games to register", result.Count);
            return Task.WhenAll(result.Select(game => _gameManager.PerformRegistration(game.Id, playerDetails)));
        });
    }
}