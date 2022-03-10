using System.Collections.Immutable;
using Sharp.Core;

namespace Sharp.Gameplay.Map;

/// <summary>
/// Field of the map. A field can contain certain objects (e.g. planets, space-stations) and has exactly 4 neighbours
/// in each direction.
/// </summary>
public class Field : IIdentifiable<string>
{
    /// <summary>
    /// A Barrier is a field that is not accessible.
    /// </summary>
    public static Field BARRIER = new("BARRIER", Int32.MaxValue, Int32.MaxValue);
    
    private Dictionary<Direction, Field> _neighbours = new(); 
    
    public string ID { get; }
    public int MovementDifficulty;
    public int RechargeMultiplicator;
    public ImmutableDictionary<Direction, Field> Neighbours => _neighbours.ToImmutableDictionary();

    public Field(string id, int movementDifficulty, int rechargeMultiplicator)
    {
        MovementDifficulty = movementDifficulty;
        RechargeMultiplicator = rechargeMultiplicator;
        ID = id;
    }

    public void AddNeighbour(Direction dir, Field field)
    {
        if (field.ID == ID)
            throw new ArgumentException("A field cannot be a neighbour to itself", nameof(field));
        if (_neighbours.ContainsKey(dir))
            throw new ArgumentException("A field in this direction already exists", nameof(dir));
        if (_neighbours.ContainsValue(field))
            throw new ArgumentException("This field is already registered as a neighbour", nameof(field));
        _neighbours.Add(dir, field);
    }
}