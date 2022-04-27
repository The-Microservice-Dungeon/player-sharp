using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Config;
using Sharp.Player.Events.Models.Robot;
using Sharp.Player.Manager;
using Sharp.Player.Repository;

namespace Sharp.Player.Events.Consumers.Robot;

public class RegenerationEventMessageHandler : IMessageHandler<RegenerationEvent>
{
    private readonly ILogger<RegenerationEventMessageHandler> _logger;
    private readonly IRobotManager _robotManager;
    private readonly ITransactionContextStore _transactionContextStore;

    public RegenerationEventMessageHandler(ILogger<RegenerationEventMessageHandler> logger, IRobotManager robotManager, ITransactionContextStore transactionContextStore)
    {
        _logger = logger;
        _robotManager = robotManager;
        _transactionContextStore = transactionContextStore;
    }

    public Task Handle(IMessageContext context, RegenerationEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);

        if (!message.Success)
        {
            _logger.LogError("Received unsuccessful Regeneration Event: {Message}", message);
            return Task.CompletedTask;
        }
        
        var robotId = context.Headers.GetString(KafkaHeaders.RobotIdHeaderName);
        if (robotId == null)
            throw new Exception("Robot is not present");
        
        _robotManager.UpdateEnergy(robotId, message.Energy);

        return Task.CompletedTask;
    }
}