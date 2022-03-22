using Sharp.Gameplay.Map;
using Sharp.Player.Controllers;

namespace Sharp.Player.Hubs;

public interface IMapHub
{
    Task MapCreated(Map map);
    Task FieldUpdated(Field field);
}