using Sharp.Gameplay.Map;

namespace Sharp.Player.Manager;

public class MapManager : IMapManager
{
    private Map? _map { get; set; } = null;

    public Map Get() => _map!;

    public Map Create(string id)
    {
        _map = new Map(id);
        return _map;
    }
}