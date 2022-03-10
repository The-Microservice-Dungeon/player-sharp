using System.Net;
using Player.Sharp.Client;
using Player.Sharp.Consumers;
using Player.Sharp.Data;
using Refit;

namespace Player.Sharp.Services;

public class PlayerRegistrationService : IHostedService
{
    private readonly IGameClient _gameClient;
    private readonly ILogger<PlayerRegistrationService> _logger;
    private readonly IPlayerCredentialsRepository _playerCredentialsRepository;

    public PlayerRegistrationService(IGameClient gameClient, ILogger<PlayerRegistrationService> logger,
        IPlayerCredentialsRepository playerCredentialsRepository)
    {
        _gameClient = gameClient;
        _logger = logger;
        _playerCredentialsRepository = playerCredentialsRepository;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested && !_playerCredentialsRepository.Exists())
        {
            try
            {
                var response =
                    await _gameClient.GetPlayerDetails(PlayerConstants.PLAYER_NAME, PlayerConstants.PLAYER_EMAIL);
                var credentials = new PlayerCredentials(response.BearerToken);
                _playerCredentialsRepository.Save(credentials);
                break;
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ApiException e)
            {
                _logger.LogError(e, "Couldn't get Player Details");
                if (e.StatusCode == HttpStatusCode.NotFound)
                    try
                    {
                        var request = new PlayerRequest(PlayerConstants.PLAYER_NAME, PlayerConstants.PLAYER_EMAIL);
                        var response = await _gameClient.CreatePlayer(request);
                        var credentials = new PlayerCredentials(response.BearerToken);
                        _playerCredentialsRepository.Save(credentials);
                        break;
                    }
                    catch (ApiException e1)
                    {
                        _logger.LogError(e1, "Couldn't create Player");
                    }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error");
                break;
            }

            // Backoff
            await Task.Delay(500, cancellationToken);
        }

        _logger.LogInformation("Successful Registered Player, Token: {Token}",
            _playerCredentialsRepository.Get().Token);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}