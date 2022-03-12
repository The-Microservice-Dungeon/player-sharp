using System.Net;
using Refit;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Data.Context;
using Sharp.Data.Model;

namespace Sharp.Player.Services;

public class GameRegistrationService : BackgroundService
{
    private readonly IGameClient _gameClient;
    private readonly ILogger<GameRegistrationService> _logger;
    private readonly IPlayerDetailsProvider _playerDetailsProvider;
    private readonly SharpDbContext _sharpDbContext;

    public GameRegistrationService(IGameClient gameClient, ILogger<GameRegistrationService> logger,
        SharpDbContext sharpDbContext, IPlayerDetailsProvider playerDetailsProvider)
    {
        _gameClient = gameClient;
        _logger = logger;
        _sharpDbContext = sharpDbContext;
        _playerDetailsProvider = playerDetailsProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var playerDetails = _playerDetailsProvider.Get();

        return GetGameIds(playerDetails.PlayerId).ContinueWith(games =>
        {
            var result = games.Result;
            _logger.LogInformation("Retreived {N} open games to register", result.Count);
            return Task.WhenAll(result.Select(id => PerformRegistration(id, playerDetails)));
        });
    }

    private async Task<List<string>> GetGameIds(string? playerId)
    {
        Task<List<GameResponse>> gamesToRegister;
        if (playerId == null)
            gamesToRegister = _gameClient.GetAllGamesOpenForRegistration();
        else
            gamesToRegister =
                _gameClient.GetAllGamesOpenForRegistrationAndPlayerNotYetRegistered(playerId);

        return (await gamesToRegister).Select(game => game.GameId).ToList();
    }

    private async Task PerformRegistration(string gameId, PlayerDetails playerDetails)
    {
        try
        {
            var result = await _gameClient.RegisterInGame(gameId, playerDetails.Token);
            var registration = new GameRegistration(gameId, result.TransactionId)
            {
                PlayerDetails = playerDetails
            };
            _sharpDbContext.GameRegistrations.Add(registration);
            await _sharpDbContext.SaveChangesAsync();
        }
        catch (ApiException e)
        {
            if (e.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.NotFound)
                return;
        }
    }
}