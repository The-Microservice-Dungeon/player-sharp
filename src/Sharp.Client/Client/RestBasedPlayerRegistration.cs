using System.Net;
using Refit;
using Sharp.Client.Model;
using Sharp.Core.Player;

namespace Sharp.Client.Client;

public class RestBasedPlayerRegistration : IPlayerRegistration
{
    private IPlayerRegistrationClient _playerRegistrationClient;

    public RestBasedPlayerRegistration(IPlayerRegistrationClient playerRegistrationClient)
    {
        _playerRegistrationClient = playerRegistrationClient;
    }

    public PlayerCredentials Register(PlayerDetails details)
    {
        PlayerRequest body = new(details.Name, details.Email);
        try
        {
            var response = _playerRegistrationClient.CreatePlayer(body).GetAwaiter().GetResult();
            return new PlayerCredentials(response.BearerToken);
        }
        catch (ApiException e)
        {
            if (e.StatusCode == HttpStatusCode.Forbidden)
                throw new DuplicatePlayerException();
            throw;
        }
    }

    public PlayerCredentials GetCredentials(PlayerDetails details)
    {
        try
        {
            var response = _playerRegistrationClient.GetPlayerDetails(details.Name, details.Email).GetAwaiter()
                .GetResult();
            return new PlayerCredentials(response.BearerToken);
        }
        catch (ApiException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
                throw new PlayerNotFoundException();
            throw;
        }
    }

    public PlayerCredentials GetCredentials(PlayerDetails details, bool registerIfNotFound)
    {
        try
        {
            var response = _playerRegistrationClient.GetPlayerDetails(details.Name, details.Email).GetAwaiter()
                .GetResult();
            return new PlayerCredentials(response.BearerToken);
        }
        catch (ApiException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
                return Register(details);
            throw;
        }
    }
}