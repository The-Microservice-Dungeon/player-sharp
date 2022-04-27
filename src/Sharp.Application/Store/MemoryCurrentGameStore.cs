using Sharp.Domain.Game;

namespace Sharp.Player.Store;

public class MemoryCurrentGameStore : ICurrentGameStore
{
    private Game? _game;

    public Game Get()
    {
        return _game ?? throw new UnsetStateException("Game is null");
    }

    public void Set(Game game)
    {
        _game = game;
    }

    public bool IsSet()
    {
        return _game != null;
    }
}