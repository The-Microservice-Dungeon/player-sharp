using Sharp.Gameplay.Robot;

namespace Sharp.Player.Repository;

public class MemoryRobotFleetStore : IRobotFleetStore
{
    private readonly List<Robot> _robotFleet = new();

    public IEnumerable<Robot> Get() => _robotFleet;

    public void Add(Robot t) => _robotFleet.Add(t);

    public void Clear() => _robotFleet.Clear();
}