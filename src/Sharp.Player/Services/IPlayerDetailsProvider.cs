using Sharp.Data.Model;

namespace Sharp.Player.Services;

public interface IPlayerDetailsProvider
{
    PlayerDetails Get();
    Task<PlayerDetails> Init();
}