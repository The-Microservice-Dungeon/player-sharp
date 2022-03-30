using Sharp.Gameplay.Map;

namespace Sharp.Player.Repository;

public class MemoryCurrentMapStore : ICurrentMapStore
{
    private Map? _map;
    
    public Map Get() => _map ?? throw new UnsetStateException("Map is null");
    
    public void Set(Map map) => _map = map;
    public bool IsSet() => _map != null;
}