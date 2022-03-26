using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Events.Models.Trading;

namespace Sharp.Player.Events.Consumers.Trading;

public class TradeBuyRobotEventHandler : IMessageHandler<TradeBuyRobotEvent>
{
    private readonly ILogger<TradeBuyRobotEventHandler> _logger;

    public TradeBuyRobotEventHandler(ILogger<TradeBuyRobotEventHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(IMessageContext context, TradeBuyRobotEvent message)
    {
        _logger.LogDebug("Received Buy Robot Trade Event: {@Message}", message);
        return Task.CompletedTask;
    }
}