using Sharp.Core;

namespace Sharp.Gameplay.Map;

/// <summary>
///     A map (also called "Gameworld").
/// </summary>
public class Map : IIdentifiable<string>
{
    public Map(string id)
    {
        Id = id;
    }

    public string Id { get; }

    public void AddField(Field field)
    {
    }
}