using Sharp.Gameplay.Map;

namespace Sharp.Player.Manager;

public interface IMapManager
{
    Map? Get();
    void Create(string id);
    void AddSpaceStation(string fieldId);
}