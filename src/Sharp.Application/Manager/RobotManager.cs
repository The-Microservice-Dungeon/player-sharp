using AutoMapper;
using Sharp.Domain.Robot;
using Sharp.Player.Events.Models.Trading;

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

    public void UpdateEnergy(string robotId, uint energy)
    {
        var robot = _robotFleetStore.Get(robotId);
        if (robot == null)
            throw new Exception($"Could not find Robot with ID ${robot}");

        robot.UpdateEnergy(energy);
    }

    public void ClearFleet()
    {
        _robotFleetStore.Clear();
    }

    public List<Robot> GetRobots()
    {
        return _robotFleetStore.Get().ToList();
    }

    public void MoveRobot(string robotId, string fieldId)
    {
        var robot = _robotFleetStore.Get(robotId);
        if (robot == null)
            throw new Exception($"Could not find Robot with ID ${robot}");
        
        var field = robot.Field.Map.GetField(fieldId);
        if (field == null)
            throw new Exception($"Could not find Field with ID ${field}");
        
        robot.Move(field);
    }

    public bool HasAnyAliveRobot() => _robotFleetStore.Get().Any(robot => robot.Alive);
}