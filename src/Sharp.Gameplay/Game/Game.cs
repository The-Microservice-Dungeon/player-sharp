using Sharp.Core.Core;

namespace Sharp.Gameplay.Game;

public class Game : IIdentifiable<string>
{
    public Game(string id)
    {
        Id = id;
    }

    public string Id { get; }
}