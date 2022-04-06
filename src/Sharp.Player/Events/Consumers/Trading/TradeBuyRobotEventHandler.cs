using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Config;
using Sharp.Player.Events.Models.Trading;
using Sharp.Player.Manager;
using Sharp.Player.Repository;

namespace Sharp.Player.Events.Consumers.Trading;

public class TradeBuyRobotEventHandler : IMessageHandler<TradeBuyRobotEvent>
{
    private readonly ITransactionContextStore _contextStore;
    private readonly ILogger<TradeBuyRobotEventHandler> _logger;
    private readonly IRobotManager _robotManager;
    private readonly IWalletStore _walletStore;

    public TradeBuyRobotEventHandler(ILogger<TradeBuyRobotEventHandler> logger, IRobotManager robotManager,
        IWalletStore walletStore, ITransactionContextStore contextStore)
    {
        _logger = logger;
        _robotManager = robotManager;
        _walletStore = walletStore;
        _contextStore = contextStore;
    }

    public async Task Handle(IMessageContext context, TradeBuyRobotEvent message)
    {
        // TODO: We do'nt Filter events yet
        var transactionId = context.Headers.GetString(KafkaHeaders.TransactionIdHeaderName);
        if (transactionId == null) throw new Exception("Transaction ID is null");

        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);

        if (!message.Success)
        {
            _logger.LogError("Received unsuccessful Buy Robot Event: {Message}", message);
            return;
        }
        
        // TODO: Shouldn't be in the handler I guess?
        _walletStore.Get().Charge(Math.Abs(message.MoneyChangeBy));

        foreach (var robot in message.Data)
        {
            _contextStore.AddContext(transactionId, ContextKeys.PlanetId, robot.Planet);
            _contextStore.AddContext(transactionId, ContextKeys.RobotId, robot.Id);
            await _robotManager.AddRobotFromTrade(robot);
        }
    }
}