using System.Collections.Immutable;
using Sharp.Core;

namespace Sharp.Gameplay.Map;

/// <summary>
///     A map (also called "Gameworld").
/// </summary>
public class Map : IIdentifiable<string>
{
    private List<Field> _fields = new();
    private List<Connection> _connections = new();
    public Map(string id)
    {
        Id = id;
    }

    public string Id { get; }
    public IImmutableList<Field> Fields => _fields.ToImmutableList();
    public IImmutableList<Connection> Connections => _connections.ToImmutableList();

    public void AddField(Field field) => _fields.Add(field);
    public void AddConnection(Connection connection) => _connections.Add(connection);
}