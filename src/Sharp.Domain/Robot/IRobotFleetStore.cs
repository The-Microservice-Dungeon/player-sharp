using Sharp.Domain.Core;

namespace Sharp.Domain.Robot;

public interface IRobotFleetStore : ICollectionStateStore<Robot>
{
    Robot? Get(string id);
}