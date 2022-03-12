using System.Net;
using Microsoft.Extensions.Options;
using Refit;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Data.Context;
using Sharp.Data.Model;
using Sharp.Player.Config;

namespace Sharp.Player.Services;

public class PlayerDetailsProvider : IPlayerDetailsProvider
{
    private readonly SharpDbContext _dbContext;
    private readonly PlayerDetailsOptions _detailsOptions;
    private readonly ILogger<PlayerDetailsProvider> _logger;
    private readonly IPlayerRegistrationClient _playerRegistrationClient;

    public PlayerDetailsProvider(IPlayerRegistrationClient playerRegistrationClient,
        IOptions<PlayerDetailsOptions> detailsOptions, ILogger<PlayerDetailsProvider> logger,
        SharpDbContext dbContext)
    {
        _playerRegistrationClient = playerRegistrationClient;
        _detailsOptions = detailsOptions.Value;
        _logger = logger;
        _dbContext = dbContext;
    }
    
    private string PlayerName => _detailsOptions.Name;
    private string PlayerEmail => _detailsOptions.Email;

    private PlayerDetails? GetFromDb(string name, string email) => _dbContext.PlayerDetails.Find(name, email);  
    
    public PlayerDetails Get()
    {
        var dbResult = GetFromDb(PlayerName, PlayerEmail);

        if (dbResult == null)
            throw new ApplicationException(
                $"A Player with the configured Name and Email does not exist yet. (Name: {PlayerName}, Email: {PlayerEmail})");
        
        return dbResult;
    }

    public async Task<PlayerDetails> Init()
    {
        _logger.LogDebug("Try to load player details from database...");
        var dbResult = GetFromDb(PlayerName, PlayerEmail);

        if (dbResult != null)
        {
            _logger.LogDebug("Successfully retreived player details: {Details}", dbResult);
            return dbResult;
        }

        var fetchedResult = await FetchPlayerDetails(PlayerName, PlayerEmail);;

        if (fetchedResult != null)
        {
            StoreInDatabase(fetchedResult);
            return fetchedResult;
        }

        var createdResult = await RegisterPlayer(PlayerName, PlayerEmail);
        StoreInDatabase(createdResult);

        return createdResult;
    }

    private void StoreInDatabase(PlayerDetails playerDetails)
    {
        _dbContext.PlayerDetails.Add(playerDetails);
        _dbContext.SaveChanges();
        if (playerDetails.PlayerId == null)
            _logger.LogWarning("Player details were stored, but a PlayerID is not present. We might fetch it later on when registering to a game");
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