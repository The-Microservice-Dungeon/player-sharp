using Player.Sharp.Client;
using Player.Sharp.Data;
using Player.Sharp.Consumers;
using Player.Sharp.Core;

namespace Player.Sharp.Services
{
    public class RobotService
    {
        private readonly ILogger<RobotService> _logger;
        private readonly IRobotRepository _robotRepository;
        private readonly IPlayerCredentialsRepository _playerCredentialsRepository;

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
    }
}
