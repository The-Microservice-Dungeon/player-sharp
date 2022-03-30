using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Client.Model;
using Sharp.Player.Events.Models.Game;
using Sharp.Player.Manager;

namespace Sharp.Player.Events.Consumers.Game;

public class GameStatusMessageHandler : IMessageHandler<GameStatusEvent>
{
    private readonly IGameManager _gameManager;
    private readonly ILogger<GameStatusMessageHandler> _logger;

    public GameStatusMessageHandler(IGameManager gameManager, ILogger<GameStatusMessageHandler> logger)
    {
        _gameManager = gameManager;
        _logger = logger;
    }

    public async Task Handle(IMessageContext context, GameStatusEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", typeof(Message).FullName, message);
        if (message.Status == GameStatus.Created) await _gameManager.PerformRegistration(message.GameId);
    }
}