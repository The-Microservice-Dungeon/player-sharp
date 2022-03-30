using Sharp.Gameplay.Map;

namespace Sharp.Player.Repository;

public class MemoryCurrentMapStore : ICurrentMapStore
{
    private Map? _map;

    public Map Get()
    {
        return _map ?? throw new UnsetStateException("Map is null");
    }

    public void Set(Map map)
    {
        _map = map;
    }

    public bool IsSet()
    {
        return _map != null;
    }
}