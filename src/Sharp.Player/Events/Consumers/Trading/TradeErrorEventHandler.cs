using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Consumers.Models.Trading;

namespace Sharp.Player.Events.Models;

public class TradeErrorEventHandler : IMessageHandler<TradeErrorEvent>
{
    private readonly ILogger<TradeErrorEventHandler> _logger;

    public TradeErrorEventHandler(ILogger<TradeErrorEventHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Handle(IMessageContext context, TradeErrorEvent message)
    {
        _logger.LogDebug("Received Error Trade Event: {@Message}", message);
    }
}