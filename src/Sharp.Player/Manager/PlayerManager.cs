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
    private readonly SharpDbContext _db;
    private readonly IPlayerDetailsProvider _playerDetails;

    public PlayerManager(SharpDbContext db, IPlayerDetailsProvider playerDetails)
    {
        _db = db;
        _playerDetails = playerDetails;
    }

    public PlayerDetails SetPlayerId(string playerId)
    {
        var details = _playerDetails.Get();
        _db.PlayerDetails.Attach(details);
        details.PlayerId = playerId;
        _db.PlayerDetails.Update(details);
        _db.SaveChanges();
        
        return details;
    }

    public PlayerDetails? ResolveRegistrationTransactionId(string transactionId)
    {
        return _db.GameRegistrations
            .Where(registration => registration.TransactionId == transactionId)
            .Select(registration => registration.PlayerDetails)
            .FirstOrDefault();
    }
}