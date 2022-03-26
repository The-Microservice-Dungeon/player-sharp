using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Consumers.Models.Trading;

namespace Sharp.Player.Events.Models;

public class TradeSellResourcesEventHandler : IMessageHandler<TradeSellResourcesEvent>
{
    private readonly ILogger<TradeSellResourcesEventHandler> _logger;

    public TradeSellResourcesEventHandler(ILogger<TradeSellResourcesEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(IMessageContext context, TradeSellResourcesEvent message)
    {
        _logger.LogDebug("Received Sell Resources Event: {@Message}", message);
    }
}