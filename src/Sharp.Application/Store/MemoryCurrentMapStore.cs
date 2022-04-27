using Sharp.Domain.Map;

namespace Sharp.Player.Store;

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