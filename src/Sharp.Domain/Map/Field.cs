using Sharp.Domain.Core;

namespace Sharp.Domain.Map;

/// <summary>
///     Field of the map. A field can contain certain objects (e.g. planets, space-stations)
/// </summary>
public class Field : IIdentifiable<string>, IMapLocatable
{
    public Field(string id, Map map)
    {
        Id = id;
        Map = map;
    }

    public Field(string id, int movementDifficulty, Map map) : this(id, map)
    {
        MovementDifficulty = movementDifficulty;
    }

    public int? MovementDifficulty { get; set; }

    public Planet? Planet { get; private set; }
    public SpaceStation? SpaceStation { get; private set; }

    public string Id { get; }

    public Map Map { get; }

    public void SetPlanet(Planet planet)
    {
        Planet = planet;
    }

    public void SetSpaceStation(SpaceStation spaceStation)
    {
        SpaceStation = spaceStation;
    }

    public Field[] GetNeighbours()
    {
        return Map.Fields[this].Select(c => c.Destination).ToArray();
    }

    public bool IsNeighbour(Field field)
    {
        return GetNeighbours().Any(n => n.Id == field.Id);
    }
}