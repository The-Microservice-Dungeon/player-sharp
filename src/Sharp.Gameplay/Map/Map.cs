using System.Collections.Immutable;
using Sharp.Core;

namespace Sharp.Gameplay.Map;

public class Map : IIdentifiable<string>
{
    private List<Field> _fields = new();
    public string ID { get; }
    public IImmutableList<Field> Fields => _fields.ToImmutableList();

    public Map(string id)
    {
        ID = id;
    }

    public Field GetFieldWithId(string id) => Fields.First(f => f.ID == id);
    public bool ExistsFieldWithId(string id) => Fields.Any(f => f.ID == id);

    public void AddField(Field field)
    {
        if (ExistsFieldWithId(field.ID))
            throw new ArgumentException("Field with this ID already exists", nameof(field));
        _fields.Add(field);
    }
}