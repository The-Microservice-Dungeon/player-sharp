using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Events.Models.Trading;

namespace Sharp.Player.Events.Consumers.Trading;

public class TradeErrorEventHandler : IMessageHandler<TradeErrorEvent>
{
    private readonly ILogger<TradeErrorEventHandler> _logger;

    public TradeErrorEventHandler(ILogger<TradeErrorEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(IMessageContext context, TradeErrorEvent message)
    {
        _logger.LogError("Received Trade Error: {@Message}", message);

        return Task.CompletedTask;
    }
}