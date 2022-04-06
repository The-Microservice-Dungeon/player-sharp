using System.Diagnostics;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Config;
using Sharp.Player.Events.Models.Game;
using Sharp.Player.Manager;
using Sharp.Player.Provider;

namespace Sharp.Player.Events.Consumers.Game;

public class PlayerStatusMessageHandler : IMessageHandler<PlayerStatusEvent>
{
    private readonly ILogger<PlayerStatusMessageHandler> _logger;
    private readonly IPlayerManager _playerManager;
    private readonly IGameManager _gameManager;

    public PlayerStatusMessageHandler(IPlayerManager playerManager, ILogger<PlayerStatusMessageHandler> logger, IGameManager gameManager)
    {
        _playerManager = playerManager;
        _logger = logger;
        _gameManager = gameManager;
    }

    public async Task Handle(IMessageContext context, PlayerStatusEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);

        var transactionId = context.Headers.GetString(KafkaHeaders.TransactionIdHeaderName) ??
                            throw new ApplicationException("There must be an transaction id");

        // Defer until registration is present
        // We now, that a robot movement event will result into a planet id set in the transaction context
        // This assumption allows us to defer the message until this id is being set.
        for (var i = 0; _gameManager.ResolveRegistration(transactionId) == null; i++)
        {
            if (i == 10)
                throw new Exception("Waiting for Game Registration took more than 10 iterations");
            // And blocking...
            await Task.Delay(100);
        }

        var registration = _gameManager.ResolveRegistration(transactionId)!;
        _playerManager.SetPlayerId(message.PlayerId);
        _logger.LogInformation("Succesfully updated PlayerId to: {PlayerId}", message.PlayerId);
    }
}