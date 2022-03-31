using System.Diagnostics;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Config;
using Sharp.Player.Events.Models.Game;
using Sharp.Player.Manager;

namespace Sharp.Player.Events.Consumers.Game;

public class PlayerStatusMessageHandler : IMessageHandler<PlayerStatusEvent>
{
    private readonly ILogger<PlayerStatusMessageHandler> _logger;
    private readonly IPlayerManager _playerManager;

    public PlayerStatusMessageHandler(IPlayerManager playerManager, ILogger<PlayerStatusMessageHandler> logger)
    {
        _playerManager = playerManager;
        _logger = logger;
    }

    public Task Handle(IMessageContext context, PlayerStatusEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);

        var transactionId = context.Headers.GetString(KafkaHeaders.TransactionIdHeaderName) ??
                            throw new ApplicationException("There must be an transaction id");

        var details = _playerManager.ResolveRegistrationTransactionId(transactionId);
        if (details == null) return Task.CompletedTask;
        
        Debug.Assert(details.Name == message.Name);
        Debug.Assert(details.PlayerId == null || details.PlayerId == message.PlayerId);

        if (details.PlayerId != message.PlayerId) _playerManager.SetPlayerId(message.PlayerId);

        return Task.CompletedTask;
    }
}