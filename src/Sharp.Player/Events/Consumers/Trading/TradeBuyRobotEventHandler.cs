using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Events.Models.Trading;
using Sharp.Player.Manager;

namespace Sharp.Player.Events.Consumers.Trading;

public class TradeBuyRobotEventHandler : IMessageHandler<TradeBuyRobotEvent>
{
    private readonly ILogger<TradeBuyRobotEventHandler> _logger;
    private readonly IRobotManager _robotManager;

    public TradeBuyRobotEventHandler(ILogger<TradeBuyRobotEventHandler> logger, IRobotManager robotManager)
    {
        _logger = logger;
        _robotManager = robotManager;
    }

    public Task Handle(IMessageContext context, TradeBuyRobotEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);

        foreach (var robot in message.Data) _robotManager.AddRobotFromTrade(robot);

        return Task.CompletedTask;
    }
}