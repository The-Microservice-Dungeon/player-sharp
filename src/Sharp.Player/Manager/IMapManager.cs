using Sharp.Gameplay.Map;

namespace Sharp.Player.Manager;

public interface IMapManager
{
    void Create(string id);
    void AddSpaceStation(string fieldId);
    void AddOpaqueField(string id, int movementDifficulty);
    void AddPlanet(string id, int movementDifficulty, ResourceType[] resourceTypes);
    Field GetField(string id);
}