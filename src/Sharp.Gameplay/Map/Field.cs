using Sharp.Core;

namespace Sharp.Gameplay.Map;

/// <summary>
///     Field of the map. A field can contain certain objects (e.g. planets, space-stations)
/// </summary>
public class Field : IIdentifiable<string>
{
    public Field(string id)
    {
        Id = id;
    }
    
    public string Id { get; }
    public Planet? Planet { get; set; }
    public SpaceStation? SpaceStation { get; set; }
}