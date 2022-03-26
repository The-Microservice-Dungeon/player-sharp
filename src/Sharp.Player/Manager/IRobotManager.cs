using Sharp.Player.Events.Models.Trading;

namespace Sharp.Player.Manager;

public interface IRobotManager
{
    Task AddRobotFromTrade(TradeRobotData boughtRobot);
    void ClearFleet();
}