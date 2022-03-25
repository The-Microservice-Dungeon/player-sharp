using System.Net;
using Microsoft.Extensions.Options;
using Refit;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Data.Context;
using Sharp.Data.Model;
using Sharp.Player.Config;

namespace Sharp.Player.Manager;

public class PlayerManager : IPlayerManager
{
    private readonly SharpDbContext _dbContext;
    private readonly PlayerDetailsOptions _detailsOptions;
    private readonly ILogger<PlayerManager> _logger;
    private readonly IPlayerRegistrationClient _playerRegistrationClient;

    public PlayerManager(IPlayerRegistrationClient playerRegistrationClient,
        IOptions<PlayerDetailsOptions> detailsOptions, ILogger<PlayerManager> logger,
        SharpDbContext dbContext)
    {
        _playerRegistrationClient = playerRegistrationClient;
        _detailsOptions = detailsOptions.Value;
        _logger = logger;
        _dbContext = dbContext;
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
        // TODO: Validate
        _logger.LogDebug("Try to load player details from database...");
        var dbResult = GetFromDb(PlayerName, PlayerEmail);

        if (dbResult != null)
        {
            _logger.LogDebug("Successfully retreived player details: {Details}", dbResult);
            return dbResult;
        }

        var fetchedResult = await FetchPlayerDetails(PlayerName, PlayerEmail);
        ;

        if (fetchedResult != null)
        {
            StoreInDatabase(fetchedResult);
            return fetchedResult;
        }

        var createdResult = await RegisterPlayer(PlayerName, PlayerEmail);
        StoreInDatabase(createdResult);

        return createdResult;
    }

    public PlayerDetails SetPlayerId(string playerId)
    {
        var details = Get();
        details.PlayerId = playerId;
        _dbContext.PlayerDetails.Update(details);
        _dbContext.SaveChanges();
        return details;
    }

    public PlayerDetails? ResolveRegistrationTransactionId(string transactionId)
    {
        return _dbContext.GameRegistrations
            .Where(registration => registration.TransactionId == transactionId)
            .Select(registration => registration.PlayerDetails)
            .FirstOrDefault();
    }

    private PlayerDetails? GetFromDb(string name, string email)
    {
        return _dbContext.PlayerDetails.Find(name, email);
    }

    private void StoreInDatabase(PlayerDetails playerDetails)
    {
        _dbContext.PlayerDetails.Add(playerDetails);
        _dbContext.SaveChanges();
        if (playerDetails.PlayerId == null)
            _logger.LogWarning(
                "Player details were stored, but a PlayerID is not present. We might fetch it later on when registering to a game");
    }

    private async Task<PlayerDetails?> FetchPlayerDetails(string name, string email)
    {
        _logger.LogDebug("Try to fetch player details from game service...");
        try
        {
            var response = await _playerRegistrationClient.GetPlayerDetails(name, email);
            PlayerDetails details = new(response.Name, response.Email, response.BearerToken);
            _logger.LogDebug("Successfully retreived player details: {Details}", details);
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
        _logger.LogDebug("Successfully created player with details: {Details}", details);
        return details;
    }
}