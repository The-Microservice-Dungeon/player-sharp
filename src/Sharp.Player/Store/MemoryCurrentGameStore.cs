using Sharp.Gameplay.Game;

namespace Sharp.Player.Repository;

public class MemoryCurrentGameStore : ICurrentGameStore
{
    private Game? _game;
    
    public Game Get() => _game ?? throw new UnsetStateException("Game is null");
    
    public void Set(Game game) => _game = game;
    public bool IsSet() => _game != null;
}