using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Consumers.Models.Trading;

namespace Sharp.Player.Events.Models;

public class TradeBuyRobotEventHandler : IMessageHandler<TradeBuyRobotEvent>
{
    private readonly ILogger<TradeBuyRobotEventHandler> _logger;

    public TradeBuyRobotEventHandler(ILogger<TradeBuyRobotEventHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Handle(IMessageContext context, TradeBuyRobotEvent message)
    {
        _logger.LogDebug("Received Buy Robot Trade Event: {@Message}", message);
    }
}