using System.Collections.Immutable;
using Sharp.Core;

namespace Sharp.Gameplay.Map;

/// <summary>
///     A map (also called "Gameworld").
/// </summary>
public class Map : IIdentifiable<string>
{
    private readonly Dictionary<Field, List<Connection>> _fields = new();

    public Map(string id)
    {
        Id = id;
    }

    // We expose a immutable dictonary as public property to ensure every modification will be done over the member
    // methods
    public ImmutableDictionary<Field, List<Connection>> Fields => _fields.ToImmutableDictionary();
    public string Id { get; }

    public Field GetOrCreateField(string id)
    {
        var field = GetField(id);
        if (field != null)
            return field;
        field = new Field(id);
        AddField(field);
        return field;
    }
    
    public Field? GetField(string id)
    {
        return _fields.Keys.FirstOrDefault(k => k.Id == id);
    }

    public void AddField(Field field)
    {
        if (_fields.Keys.Any(k => k.Id == field.Id))
            throw new ArgumentException("A field with this Id already exists", nameof(field));
        _fields.Add(field, new List<Connection>());
    }
}