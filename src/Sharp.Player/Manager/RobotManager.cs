using AutoMapper;
using Sharp.Gameplay.Robot;
using Sharp.Player.Events.Models.Trading;
using Sharp.Player.Repository;

namespace Sharp.Player.Manager;

public class RobotManager : IRobotManager
{
    private readonly ILogger<RobotManager> _logger;
    private readonly IMapManager _mapManager;
    private readonly IMapper _mapper;
    private readonly IRobotFleetStore _robotFleetStore;

    public RobotManager(IMapManager mapManager, ILogger<RobotManager> logger, IMapper mapper, IRobotFleetStore robotFleetStore)
    {
        _mapManager = mapManager;
        _logger = logger;
        _mapper = mapper;
        _robotFleetStore = robotFleetStore;
    }

    public Task AddRobotFromTrade(TradeRobotData boughtRobot)
    {
        _logger.LogDebug("Adding Robot {@Robot} to fleet", boughtRobot);

        var attributes = _mapper.Map<RobotAttributes>(boughtRobot);
        var field = _mapManager.GetField(boughtRobot.Planet);

        var robot = new Robot(boughtRobot.Id, boughtRobot.Alive, attributes, field);
        _robotFleetStore.Add(robot);

        _logger.LogDebug("Robot {@Robot} added to fleet", robot);

        return Task.CompletedTask;
    }

    public void ClearFleet()
    {
        _robotFleetStore.Clear();
    }
}