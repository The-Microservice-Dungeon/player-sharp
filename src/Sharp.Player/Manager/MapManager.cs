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

    public void Create(string id)
    {
        _map = new Map(id);
    }

    public void AddSpaceStation(string fieldId)
    {
        if (_map == null)
            // Shouldn't happen
            throw new ApplicationException("Map is null.");

        var field = _map.GetField(fieldId) ?? new Field(fieldId);

        if (field.SpaceStation != null)
            return;

        field.SetSpaceStation(new SpaceStation());
        _map.AddField(field);
    }
}