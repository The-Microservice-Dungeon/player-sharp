using Sharp.Domain.Robot;
using Sharp.Player.Events.Models.Trading;

namespace Sharp.Player.Manager;

public interface IRobotManager
{
    Task AddRobotFromTrade(TradeRobotData boughtRobot);
    bool HasAnyAliveRobot();
    List<Robot> GetRobots();
    void MoveRobot(string robotId, string fieldId);
    void UpdateEnergy(string robotId, uint energy);
    void ClearFleet();
}