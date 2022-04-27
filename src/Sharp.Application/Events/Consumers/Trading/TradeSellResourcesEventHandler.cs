using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Domain.Trading;
using Sharp.Player.Events.Models.Trading;

namespace Sharp.Player.Events.Consumers.Trading;

public class TradeSellResourcesEventHandler : IMessageHandler<TradeSellResourcesEvent>
{
    private readonly ILogger<TradeSellResourcesEventHandler> _logger;
    private readonly IWalletStore _walletStore;

    public TradeSellResourcesEventHandler(ILogger<TradeSellResourcesEventHandler> logger, IWalletStore walletStore)
    {
        _logger = logger;
        _walletStore = walletStore;
    }

    public Task Handle(IMessageContext context, TradeSellResourcesEvent message)
    {
        // TODO: We do'nt Filter events yet
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);

        if (!message.Success)
        {
            _logger.LogError("Received unsuccessful Resource Sell Event: {Message}", message);
            return Task.CompletedTask;
        }
        
        // TODO: Shouldn't be in the handler I guess?
        _walletStore.Get().Deposit(message.MoneyChangeBy);

        return Task.CompletedTask;
    }
}