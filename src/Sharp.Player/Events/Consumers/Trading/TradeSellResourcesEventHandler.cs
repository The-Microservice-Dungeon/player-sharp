using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Events.Models.Trading;

namespace Sharp.Player.Events.Consumers.Trading;

public class TradeSellResourcesEventHandler : IMessageHandler<TradeSellResourcesEvent>
{
    private readonly ILogger<TradeSellResourcesEventHandler> _logger;

    public TradeSellResourcesEventHandler(ILogger<TradeSellResourcesEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(IMessageContext context, TradeSellResourcesEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", typeof(Message).FullName, message);

        return Task.CompletedTask;
    }
}