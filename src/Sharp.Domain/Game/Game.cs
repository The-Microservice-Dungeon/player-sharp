using Sharp.Domain.Core;

namespace Sharp.Domain.Game;

public class Game : IIdentifiable<string>
{
    public Game(string id)
    {
        Id = id;
    }

    public string Id { get; }
}