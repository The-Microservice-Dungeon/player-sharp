using Player.Sharp.Services;

namespace Player.Sharp.Gameplay;

public interface IRoundLifecycleHandler
{
    public bool CheckCondition();

    public void OnRoundStart()
    {
    }

    public void OnRoundEnd()
    {
    }
}

public class NoRobotLifecycleHandler : IRoundLifecycleHandler
{
    private readonly CommandService _commandService;
    private readonly ILogger _logger;
    private readonly RobotService _robotService;

    public NoRobotLifecycleHandler(ILogger<NoRobotLifecycleHandler> logger, CommandService commandService,
        RobotService robotService)
    {
        _logger = logger;
        _commandService = commandService;
        _robotService = robotService;
    }

    public bool CheckCondition()
    {
        return !_robotService.HasAnyRobot();
    }

    public void OnRoundStart()
    {
        _commandService.BuyRobot(1);
    }
}