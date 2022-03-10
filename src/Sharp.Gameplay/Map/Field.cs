using Sharp.Core;

namespace Sharp.Gameplay.Map;

/// <summary>
///     Field of the map. A field can contain certain objects (e.g. planets, space-stations)
/// </summary>
public class Field : IIdentifiable<string>
{
    /// <summary>
    ///     A barrier is an unpassable field. For example it could be the border of the map.
    /// </summary>
    public static Field BARRIER = new("BARRIER", int.MaxValue);

    public int MovementDifficulty;

    public Field(string id)
    {
        Id = id;
    }

    public Field(string id, int movementDifficulty) : this(id)
    {
        MovementDifficulty = movementDifficulty;
    }
    
    public string Id { get; }
}