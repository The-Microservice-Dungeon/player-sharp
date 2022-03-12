using Sharp.Data.Model;

namespace Sharp.Player.Manager;

public interface IPlayerManager
{
    PlayerDetails Get();
    Task<PlayerDetails> Init();

    PlayerDetails SetPlayerId(string playerId);
    PlayerDetails? ResolveRegistrationTransactionId(string transactionId);
}