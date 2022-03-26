using System.Net;
using Microsoft.Extensions.Options;
using Refit;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Data.Contexts;
using Sharp.Data.Models;
using Sharp.Player.Config;

namespace Sharp.Player.Manager;

public class PlayerManager : IPlayerManager
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PlayerDetailsOptions _detailsOptions;
    private readonly ILogger<PlayerManager> _logger;
    private readonly IPlayerRegistrationClient _playerRegistrationClient;

    public PlayerManager(IPlayerRegistrationClient playerRegistrationClient,
        IOptions<PlayerDetailsOptions> detailsOptions, ILogger<PlayerManager> logger,
        IServiceScopeFactory scopeFactory)
    {
        _playerRegistrationClient = playerRegistrationClient;
        _detailsOptions = detailsOptions.Value;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    private string PlayerName => _detailsOptions.Name;
    private string PlayerEmail => _detailsOptions.Email;

    public PlayerDetails Get()
    {
        var dbResult = GetFromDb(PlayerName, PlayerEmail);

        if (dbResult == null)
            throw new ApplicationException(
                $"A Player with the configured Name and Email does not exist yet. (Name: {PlayerName}, Email: {PlayerEmail})");

        return dbResult;
    }

    // TODO: it should be replicated by the workflow rather than an explicit call of init. We should maybe make the 
    // constructor private and provide a static method which returns an instance of the playermanager with initalized
    // state.
    public async Task<PlayerDetails> Init()
    {
        _logger.LogDebug("Try to load player details from database...");
        var dbResult = GetFromDb(PlayerName, PlayerEmail);

        if (dbResult != null)
        {
            _logger.LogDebug("Successfully retreived player details: {@Details}", dbResult);

            // In the meantime the credentials COULD have been changed. Therefore we're going to validate them and only
            // use them when they are still valid
            if (await ValidatePlayerDetails(dbResult))
                return dbResult;
            await RemoveDetails(dbResult);
        }

        var fetchedResult = await FetchPlayerDetails(PlayerName, PlayerEmail);

        if (fetchedResult != null)
        {
            await StoreInDatabase(fetchedResult);
            return fetchedResult;
        }

        var createdResult = await RegisterPlayer(PlayerName, PlayerEmail);
        await StoreInDatabase(createdResult);

        return createdResult;
    }

    public PlayerDetails SetPlayerId(string playerId)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SharpDbContext>();
            var details = Get();
            details.PlayerId = playerId;
            db.PlayerDetails.Update(details);
            db.SaveChanges();
            return details;
        }
    }

    public PlayerDetails? ResolveRegistrationTransactionId(string transactionId)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SharpDbContext>();
            return db.GameRegistrations
                .Where(registration => registration.TransactionId == transactionId)
                .Select(registration => registration.PlayerDetails)
                .FirstOrDefault();
        }
    }

    private async Task<bool> ValidatePlayerDetails(PlayerDetails playerDetails)
    {
        var fetched = await FetchPlayerDetails(playerDetails.Name, playerDetails.Email);
        return fetched != null && fetched.Token == playerDetails.Token && fetched.Email == playerDetails.Email &&
               fetched.Name == playerDetails.Name;
    }

    private PlayerDetails? GetFromDb(string name, string email)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SharpDbContext>();
            return db.PlayerDetails.Find(name, email);
        }
    }

    private async Task StoreInDatabase(PlayerDetails playerDetails)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SharpDbContext>();
            await db.PlayerDetails.AddAsync(playerDetails);
            await db.SaveChangesAsync();
            if (playerDetails.PlayerId == null)
                _logger.LogWarning(
                    "Player details were stored, but a PlayerID is not present. We might fetch it later on when registering to a game");
        }
    }

    private async Task RemoveDetails(PlayerDetails playerDetails)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SharpDbContext>();
            db.PlayerDetails.Remove(playerDetails);
            await db.SaveChangesAsync();
        }
    }

    private async Task<PlayerDetails?> FetchPlayerDetails(string name, string email)
    {
        _logger.LogDebug("Try to fetch player details from game service...");
        try
        {
            var response = await _playerRegistrationClient.GetPlayerDetails(name, email);
            PlayerDetails details = new(response.Name, response.Email, response.BearerToken);
            _logger.LogDebug("Successfully retreived player details: {@Details}", details);
            return details;
        }
        catch (ApiException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
                return null;

            throw;
        }
    }

    private async Task<PlayerDetails> RegisterPlayer(string name, string email)
    {
        _logger.LogDebug("Try to register player in game service...");
        var response = await _playerRegistrationClient.CreatePlayer(new PlayerRequest(name, email));
        var details = new PlayerDetails(response.Name, response.Email,
            response.BearerToken);
        _logger.LogDebug("Successfully created player with details: {@Details}", details);
        return details;
    }
}