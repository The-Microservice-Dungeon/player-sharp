using Sharp.Core;
using Sharp.Data.Model;

namespace Sharp.Player.Manager;

public interface IGameManager
{
    Task PerformRegistration(string gameId, PlayerDetails playerDetails);

    Task<List<Game>> GetAvailableGames();
}