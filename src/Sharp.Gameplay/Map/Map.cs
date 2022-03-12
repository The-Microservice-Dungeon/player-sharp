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

    public Field? GetField(string id)
    {
        return _fields.Find(f => f.Id == id);
    }
    
    public void AddField(Field field)
    {
        if (_fields.Exists(f => f.Id == field.Id))
            throw new ArgumentException("A field with this Id already exists", nameof(field));
        _fields.Add(field);
    }

    public void AddConnection(Connection connection)
    {
        if (_connections.Contains(connection))
            throw new ArgumentException("This connection already exists", nameof(connection));
        _connections.Add(connection);
    }
}