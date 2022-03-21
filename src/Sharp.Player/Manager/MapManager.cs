using Sharp.Gameplay.Map;

namespace Sharp.Player.Manager;

public class MapManager : IMapManager
{
    // TODO: Get rid of that nullability. 
    private Map? _map { get; set; }

    public Map? Get()
    {
        return _map;
    }

    public Map Create(string id)
    {
        _map = new Map(id);
        return _map;
    }

    public void AddSpaceStation(string fieldId)
    {
        if (_map == null)
            // Shouldn't happen
            throw new ApplicationException("Map is null.");
        var field = _map.GetField(fieldId) ?? new Field(fieldId);
        field.SpaceStation = new SpaceStation();
        _map.SetField(field);
    }
}