﻿using System.Net;
using Microsoft.Extensions.Options;
using Refit;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Data.Contexts;
using Sharp.Data.Models;
using Sharp.Player.Config;

namespace Sharp.Player.Provider;

public class PlayerDetailsProvider : IPlayerDetailsProvider
{
    private readonly SharpDbContext _dbContext;
    private readonly ILogger<PlayerDetailsProvider> _logger;
    private readonly IPlayerRegistrationClient _registrationClient;
    private readonly IOptions<PlayerDetailsOptions> _detailsOptions;

    public PlayerDetailsProvider(
        SharpDbContext dbContext,
        ILogger<PlayerDetailsProvider> logger,
        IPlayerRegistrationClient registrationClient,
        IOptions<PlayerDetailsOptions> detailsOptions
    )
    {
        _dbContext = dbContext;
        _logger = logger;
        _registrationClient = registrationClient;
        _detailsOptions = detailsOptions;
    }

    public PlayerDetails Get()
    {
        return GetAsync().GetAwaiter().GetResult();
    }

    public async Task<PlayerDetails> GetAsync()
    {
        var playerName = _detailsOptions.Value.Name;
        var playerEmail = _detailsOptions.Value.Email;

        _logger.LogDebug("Try to load player details from database...");
        var dbResult = GetFromDb(playerName, playerEmail);

        if (dbResult != null)
        {
            _logger.LogDebug("Successfully retreived player details: {@Details}", dbResult);

            // In the meantime the credentials COULD have been changed. Therefore we're going to validate them and only
            // use them when they are still valid
            if (await ValidatePlayerDetails(dbResult))
                return dbResult;
            await RemoveDetails(dbResult);
        }

        var fetchedResult = await FetchPlayerDetails(playerName, playerEmail);

        if (fetchedResult != null)
        {
            await StoreInDatabase(fetchedResult);
            return fetchedResult;
        }

        var createdResult = await RegisterPlayer(playerName, playerEmail);
        await StoreInDatabase(createdResult);

        return createdResult;
    }

    private async Task RemoveDetails(PlayerDetails playerDetails)
    {
        _dbContext.PlayerDetails.Remove(playerDetails);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<bool> ValidatePlayerDetails(PlayerDetails playerDetails)
    {
        var fetched = await FetchPlayerDetails(playerDetails.Name, playerDetails.Email);
        return fetched != null && fetched.Token == playerDetails.Token && fetched.Email == playerDetails.Email &&
               fetched.Name == playerDetails.Name;
    }

    private async Task StoreInDatabase(PlayerDetails playerDetails)
    {
        await _dbContext.PlayerDetails.AddAsync(playerDetails);
        await _dbContext.SaveChangesAsync();
        if (playerDetails.PlayerId == null)
            _logger.LogWarning(
                "Player details were stored, but a PlayerID is not present. We might fetch it later on when registering to a game");
    }

    private PlayerDetails? GetFromDb(string name, string email)
    {
        return _dbContext.PlayerDetails.Find(name, email);
    }

    private async Task<PlayerDetails?> FetchPlayerDetails(string name, string email)
    {
        _logger.LogDebug("Try to fetch player details from game service...");
        try
        {
            var response = await _registrationClient.GetPlayerDetails(name, email);
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

        var response = await _registrationClient.CreatePlayer(new PlayerRequest(name, email));
        var details = new PlayerDetails(response.Name, response.Email, response.BearerToken);

        _logger.LogDebug("Successfully created player with details: {@Details}", details);
        return details;
    }
}