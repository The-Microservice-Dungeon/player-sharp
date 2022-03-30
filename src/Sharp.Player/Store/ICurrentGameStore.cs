using Sharp.Gameplay.Game;

namespace Sharp.Player.Repository;

public interface ICurrentGameStore : ISingleStateStore<Game>
{
}