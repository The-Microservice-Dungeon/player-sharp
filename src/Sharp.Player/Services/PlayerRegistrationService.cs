namespace Sharp.Player.Services;

public class PlayerRegistrationService : IHostedService
{
    private readonly IPlayerDetailsProvider _playerDetailsProvider;
    private readonly ILogger<PlayerRegistrationService> _logger;

    public PlayerRegistrationService(IPlayerDetailsProvider playerDetailsProvider, ILogger<PlayerRegistrationService> logger)
    {
        _playerDetailsProvider = playerDetailsProvider;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Started Service: {Service}", nameof(PlayerRegistrationService));
        return _playerDetailsProvider.Init();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}