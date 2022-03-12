using Sharp.Player.Manager;

namespace Sharp.Player.Services;

public class PlayerRegistrationService : IHostedService
{
    private readonly IPlayerManager _playerManager;
    private readonly ILogger<PlayerRegistrationService> _logger;

    public PlayerRegistrationService(IPlayerManager playerManager, ILogger<PlayerRegistrationService> logger)
    {
        _playerManager = playerManager;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Started Service: {Service}", nameof(PlayerRegistrationService));
        return _playerManager.Init();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}