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

    public int? MovementDifficulty { get; }

    public Planet? Planet { get; private set; }
    public SpaceStation? SpaceStation { get; private set; }

    // TODO: We could also use something like this but this would come with additional complexity as we would have to
    //  deal with polymorphism. Maybe this is not a problem, maybe it is? What is better for extensiblity? Does it make
    //  more sens to be explicit here?
    /*private List<IFieldLocatable> _content = new();
    // We expose a immutable list as public property to ensure every modification will be done over the member
    // methods
    public ImmutableList<IFieldLocatable> Content => _content.ToImmutableList();*/

    public string Id { get; }

    public void SetPlanet(Planet planet)
    {
        Planet = planet;
    }

    public void SetSpaceStation(SpaceStation spaceStation)
    {
        SpaceStation = spaceStation;
    }
}