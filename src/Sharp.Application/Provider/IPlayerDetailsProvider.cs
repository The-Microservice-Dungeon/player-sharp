using Sharp.Infrastructure.Persistence.Models;

namespace Sharp.Player.Provider;

public interface IPlayerDetailsProvider
{
    PlayerDetails Get();
    Task<PlayerDetails> GetAsync();
}