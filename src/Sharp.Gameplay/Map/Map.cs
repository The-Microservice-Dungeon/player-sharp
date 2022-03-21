using System.Collections.Immutable;
using Sharp.Core;

namespace Sharp.Gameplay.Map;

/// <summary>
///     A map (also called "Gameworld").
/// </summary>
public class Map : IIdentifiable<string>
{
    private readonly List<Field> _fields = new();

    public Map(string id)
    {
        Id = id;
    }

    public ImmutableList<Field> Fields => _fields.ToImmutableList();

    public string Id { get; }

    public Field? GetField(string id)
    {
        return _fields.Find(f => f.Id == id);
    }

    public Field AddField(Field field)
    {
        if (_fields.Exists(f => f.Id == field.Id))
            throw new ArgumentException("A field with this Id already exists", nameof(field));
        _fields.Add(field);
        return field;
    }

    public Field SetField(Field field)
    {
        var existingField = _fields.Find(f => f.Id == field.Id);
        if (existingField != null)
            _fields.Remove(existingField);
        _fields.Add(field);
        return field;
    }
}