using Sharp.Client.Client;
using Sharp.Data.Model;

namespace Sharp.Player.Services;

public class GameRegistrationService : BackgroundService
{
    private readonly IGameClient _gameClient;
    private readonly ILogger<GameRegistrationService> _logger;
    private readonly GameRegistration _registrationDb;

    public GameRegistrationService(IGameClient gameClient, ILogger<GameRegistrationService> logger, GameRegistration registrationDb)
    {
        _gameClient = gameClient;
        _logger = logger;
        _registrationDb = registrationDb;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}