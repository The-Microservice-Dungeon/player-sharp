using Sharp.Player.Manager;

namespace Sharp.Player.Services;

public class PlayerRegistrationService : BackgroundService
{
    private readonly IPlayerManager _playerManager;
    private readonly ILogger<PlayerRegistrationService> _logger;

    public PlayerRegistrationService(IPlayerManager playerManager, ILogger<PlayerRegistrationService> logger)
    {
        _playerManager = playerManager;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Started {Service}", nameof(PlayerRegistrationService));
        await _playerManager.Init();
    } 
}