using Sharp.Data.Models;
using Sharp.Gameplay.Game;

namespace Sharp.Player.Manager;

public interface IGameManager
{
    Task PerformRegistration(string gameId, PlayerDetails playerDetails);

    Task<List<Game>> GetAvailableGames();
    Task<List<Game>> GetRegisteredGames();
}