using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Config;
using Sharp.Player.Events.Models.Trading;
using Sharp.Player.Manager;
using Sharp.Player.Repository;

namespace Sharp.Player.Events.Consumers.Trading;

public class TradeBuyRobotEventHandler : IMessageHandler<TradeBuyRobotEvent>
{
    private readonly ILogger<TradeBuyRobotEventHandler> _logger;
    private readonly IRobotManager _robotManager;
    private readonly IWalletStore _walletStore;
    private readonly ITransactionContextStore _contextStore;

    public TradeBuyRobotEventHandler(ILogger<TradeBuyRobotEventHandler> logger, IRobotManager robotManager, IWalletStore walletStore, ITransactionContextStore contextStore)
    {
        _logger = logger;
        _robotManager = robotManager;
        _walletStore = walletStore;
        _contextStore = contextStore;
    }

    public Task Handle(IMessageContext context, TradeBuyRobotEvent message)
    {
        // TODO: We do'nt Filter events yet
        var transactionId = context.Headers.GetString(KafkaHeaders.TransactionIdHeaderName);
        if (transactionId == null) throw new Exception("Transaction ID is null");
        
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);
        
        // TODO: Shouldn't be in the handler I guess?
        _walletStore.Get().Charge(Math.Abs(message.MoneyChangeBy));

        foreach (var robot in message.Data)
        {
            _contextStore.AddContext(transactionId, ContextKeys.PlanetId, robot.Planet);
            _contextStore.AddContext(transactionId, ContextKeys.RobotId, robot.Id);
            _robotManager.AddRobotFromTrade(robot);
        }

        return Task.CompletedTask;
    }
}