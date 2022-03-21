using System.Collections.Immutable;
using Sharp.Core;

namespace Sharp.Gameplay.Map;

/// <summary>
///     Field of the map. A field can contain certain objects (e.g. planets, space-stations)
/// </summary>
public class Field : IIdentifiable<string>
{
    private readonly List<Connection> _connections = new();
    public int MovementDifficulty;

    public Field(string id)
    {
        Id = id;
    }

    public Field(string id, int movementDifficulty) : this(id)
    {
        MovementDifficulty = movementDifficulty;
    }

    public Planet? Planet { get; set; }
    public SpaceStation? SpaceStation { get; set; }
    public ImmutableList<Connection> Connections => _connections.ToImmutableList();

    public string Id { get; }

    public void AddConnection(Connection connection)
    {
        if (_connections.Contains(connection))
            throw new ArgumentException("This connection already exists", nameof(connection));
        _connections.Add(connection);
    }
}