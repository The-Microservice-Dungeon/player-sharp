using System.Diagnostics;
using Sharp.Data.Contexts;
using Sharp.Data.Models;
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
        Debug.Assert(details.PlayerId == null || details.PlayerId == playerId);
        
        _db.PlayerDetails.Attach(details);
        details.PlayerId = playerId;
        _db.PlayerDetails.Update(details);
        _db.SaveChanges();

        return details;
    }
}