using Sharp.Domain.Map;

namespace Sharp.Player.Manager;

public interface IMapManager
{
    void Create(string id);
    void AddSpaceStation(string fieldId);
    void AddOpaqueField(string id, int movementDifficulty);
    void AddPlanet(string id, int movementDifficulty, List<ResourceType> resourceTypes);
    void AddNeighbour(string fieldId, string neighbourId, int movementDifficulty);
    Field GetField(string id);
}