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
    
    public Field(string id, int movementDifficulty) : this(id)
    {
        MovementDifficulty = movementDifficulty;
    }
    
    public string Id { get; }
    public int MovementDifficulty = 0;
    public Planet? Planet { get; set; }
    public SpaceStation? SpaceStation { get; set; }
}