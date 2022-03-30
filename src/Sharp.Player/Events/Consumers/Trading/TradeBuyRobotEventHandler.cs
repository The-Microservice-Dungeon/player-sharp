using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Events.Models.Trading;
using Sharp.Player.Manager;
using Sharp.Player.Repository;

namespace Sharp.Player.Events.Consumers.Trading;

public class TradeBuyRobotEventHandler : IMessageHandler<TradeBuyRobotEvent>
{
    private readonly ILogger<TradeBuyRobotEventHandler> _logger;
    private readonly IRobotManager _robotManager;
    private readonly IWalletStore _walletStore;

    public TradeBuyRobotEventHandler(ILogger<TradeBuyRobotEventHandler> logger, IRobotManager robotManager, IWalletStore walletStore)
    {
        _logger = logger;
        _robotManager = robotManager;
        _walletStore = walletStore;
    }

    public Task Handle(IMessageContext context, TradeBuyRobotEvent message)
    {
        // TODO: We do'nt Filter events yet
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);
        
        // TODO: Shouldn't be in the handler I guess?
        _walletStore.Get().Charge(Math.Abs(message.MoneyChangeBy));
        
        foreach (var robot in message.Data) _robotManager.AddRobotFromTrade(robot);

        return Task.CompletedTask;
    }
}