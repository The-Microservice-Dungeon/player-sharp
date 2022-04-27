using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Events.Models.Game;
using Sharp.Player.Manager;

namespace Sharp.Player.Events.Consumers.Game;

public class RoundStatusMessageHandler : IMessageHandler<RoundStatusEvent>
{
    private readonly ICommandManager _commandManager;
    private readonly ILogger<RoundStatusMessageHandler> _logger;
    private readonly IRobotManager _robotManager;

    public RoundStatusMessageHandler(ILogger<RoundStatusMessageHandler> logger, ICommandManager commandManager, IRobotManager robotManager)
    {
        _logger = logger;
        _commandManager = commandManager;
        _robotManager = robotManager;
    }

    public async Task Handle(IMessageContext context, RoundStatusEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);

        _commandManager.GameId = message.GameId;
        if (message.RoundStatus == RoundStatus.Started)
        {
            // TODO: Strategies should be implemented in favor of doing this procedural. This is just for testing
            // TODO: We should wait until dangling commands are processed 
            if (!_robotManager.HasAnyAliveRobot())
            {
                await _commandManager.BuyRobot();
            }
            else
            {
                await _commandManager.RandomMovement();
            }
        }
    }
}