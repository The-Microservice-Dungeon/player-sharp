using Sharp.Data.Models;

namespace Sharp.Player.Manager;

public interface IPlayerManager
{
    PlayerDetails SetPlayerId(string playerId);
}