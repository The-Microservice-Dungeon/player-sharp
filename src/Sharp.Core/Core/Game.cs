namespace Sharp.Core;

public class Game : IIdentifiable<string>
{
    public string Id { get; }

    public Game(string id)
    {
        Id = id;
    }
}