using System.Net;
using Microsoft.Extensions.Options;
using Refit;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Data.Contexts;
using Sharp.Data.Models;
using Sharp.Player.Config;
using Sharp.Player.Provider;

namespace Sharp.Player.Manager;

public class PlayerManager : IPlayerManager
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PlayerManager(
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public PlayerDetails SetPlayerId(string playerId)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SharpDbContext>();
        var detailsProvider = scope.ServiceProvider.GetRequiredService<IPlayerDetailsProvider>();
        
        var details = detailsProvider.Get();
        db.PlayerDetails.Attach(details);
        details.PlayerId = playerId;
        db.PlayerDetails.Update(details);
        db.SaveChanges();
        
        return details;
    }

    public PlayerDetails? ResolveRegistrationTransactionId(string transactionId)
    {
        using var scope = _scopeFactory.CreateScope();
        
        var db = scope.ServiceProvider.GetRequiredService<SharpDbContext>();
        
        return db.GameRegistrations
            .Where(registration => registration.TransactionId == transactionId)
            .Select(registration => registration.PlayerDetails)
            .FirstOrDefault();
    }
}