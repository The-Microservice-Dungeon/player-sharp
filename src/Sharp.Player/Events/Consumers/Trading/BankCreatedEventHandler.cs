using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Gameplay.Trading;
using Sharp.Player.Events.Models.Trading;
using Sharp.Player.Manager;
using Sharp.Player.Provider;
using Sharp.Player.Repository;

namespace Sharp.Player.Events.Consumers.Trading;

public class BankCreatedEventHandler : IMessageHandler<BankCreatedEvent>
{
    private readonly ILogger<BankCreatedEventHandler> _logger;
    private readonly IWalletStore _walletStore;
    private readonly IPlayerDetailsProvider _playerDetailsProvider;

    public BankCreatedEventHandler(ILogger<BankCreatedEventHandler> logger, IWalletStore walletStore, IPlayerDetailsProvider playerDetailsProvider)
    {
        _logger = logger;
        _walletStore = walletStore;
        _playerDetailsProvider = playerDetailsProvider;
    }

    public async Task Handle(IMessageContext context, BankCreatedEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);
        
        // TODO: This should not be in the handler I guess
        var playerDetails = await _playerDetailsProvider.GetAsync();
        if (message.PlayerId == playerDetails.PlayerId)
        {
            _walletStore.Set(new Wallet(message.Money));
        }
    }
}