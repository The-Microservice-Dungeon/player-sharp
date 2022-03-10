using Sharp.Core;

namespace Sharp.Gameplay.Map;

/// <summary>
///     A map (also called "Gameworld").
/// </summary>
public class Map : IIdentifiable<string>
{
    public Map(string id)
    {
        ID = id;
    }

    public string ID { get; }

    public void AddField(Field field)
    {
    }
}