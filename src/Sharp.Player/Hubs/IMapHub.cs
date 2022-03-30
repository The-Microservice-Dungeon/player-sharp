using Sharp.Gameplay.Map;

namespace Sharp.Player.Hubs;

public interface IMapHub
{
    Task MapCreated(Map map);
    Task FieldUpdated(Field field);
}