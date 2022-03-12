using Sharp.Gameplay.Map;

namespace Sharp.Player.Manager;

public interface IMapManager
{
    Map Get();
    Map Create(string id);
}