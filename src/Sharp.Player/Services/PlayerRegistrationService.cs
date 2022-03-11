using System.Net;
using Microsoft.Extensions.Options;
using Refit;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Data.Context;
using Sharp.Data.Model;
using Sharp.Player.Config;

namespace Sharp.Player.Services;

public class PlayerRegistrationService : IHostedService
{
    private readonly IPlayerRegistrationClient _playerRegistrationClient;
    private readonly PlayerDetailsOptions _detailsOptions;
    private readonly ILogger<PlayerRegistrationService> _logger;
    private readonly SharpDbContext _dbContext;

    public PlayerRegistrationService(IPlayerRegistrationClient playerRegistrationClient,
        IOptions<PlayerDetailsOptions> detailsOptions, ILogger<PlayerRegistrationService> logger, SharpDbContext dbContext)
    {
        _playerRegistrationClient = playerRegistrationClient;
        _detailsOptions = detailsOptions.Value;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogDebug("Started {Service}", nameof(PlayerRegistrationService));

            var name = _detailsOptions.Name;
            var email = _detailsOptions.Email;
            _logger.LogInformation("Using player details (Name: {Name}, Email: {Email}", name, email);

            var details = _dbContext.PlayerDetails.Find(name, email);
            if (details != null)
            {
                _logger.LogWarning("Player details are already present. It could be invalidated by the game service. Hope for the best!");
                break;
            }

            _logger.LogDebug("Try to retreive player details");
            try
            {
                var response = await _playerRegistrationClient.GetPlayerDetails(name, email);
                _logger.LogInformation("Successfully loaded player details");
                _dbContext.PlayerDetails.Add(new(response.Name, response.Email, response.BearerToken));
                await _dbContext.SaveChangesAsync(cancellationToken);
                break;
            }
            catch (ApiException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogDebug("The player has not been registered yet.");

                    _logger.LogDebug("Register player");
                    var response = await _playerRegistrationClient.CreatePlayer(new(name, email));
                    _logger.LogInformation("Successfully registered player");
                    _dbContext.PlayerDetails.Add(new(response.Name, response.Email, response.BearerToken));
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    break;
                }

                throw;
            }

            // TODO: We could backoff and retry here. Don't know if this is a good idea...
            // await Task.Delay(500, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopped {Service}", nameof(PlayerRegistrationService));
        return Task.CompletedTask;
    }
}