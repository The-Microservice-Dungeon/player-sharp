using Player.Sharp.Core;
using Player.Sharp.Data;

namespace Player.Sharp.Services;

public class RobotService
{
    private readonly ILogger<RobotService> _logger;
    private readonly IPlayerCredentialsRepository _playerCredentialsRepository;
    private readonly IRobotRepository _robotRepository;

    public RobotService(
        ILogger<RobotService> logger,
        IRobotRepository robotRepository,
        IPlayerCredentialsRepository playerCredentialsRepository)
    {
        _logger = logger;
        _robotRepository = robotRepository;
        _playerCredentialsRepository = playerCredentialsRepository;
    }

    public Robot AddRobot(string robotId)
    {
        var robot = new Robot(robotId);
        _robotRepository.Save(robot);
        return robot;
    }

    public bool HasAnyRobot()
    {
        return _robotRepository.Count() > 0;
    }

    public void RemoveRobot(string robotId)
    {
        _robotRepository.RemoveById(robotId);
    }
}