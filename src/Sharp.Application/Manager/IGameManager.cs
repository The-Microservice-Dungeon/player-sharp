using Sharp.Domain.Game;
using Sharp.Infrastructure.Persistence.Models;

namespace Sharp.Player.Manager;

public interface IGameManager
{
    Task PerformRegistration(string gameId);
    GameRegistration? ResolveRegistration(string transactionId);

    Task<List<Game>> GetAvailableGames();
    Task<List<Game>> GetRegisteredGames();
}