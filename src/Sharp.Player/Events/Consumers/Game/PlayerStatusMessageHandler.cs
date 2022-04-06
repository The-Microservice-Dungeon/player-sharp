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

    public Task Handle(IMessageContext context, PlayerStatusEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);

        var transactionId = context.Headers.GetString(KafkaHeaders.TransactionIdHeaderName) ??
                            throw new ApplicationException("There must be an transaction id");

        var registration = _gameManager.ResolveRegistration(transactionId);
        if (registration != null)
        {
            _playerManager.SetPlayerId(message.PlayerId);
        }

        return Task.CompletedTask;
    }
}