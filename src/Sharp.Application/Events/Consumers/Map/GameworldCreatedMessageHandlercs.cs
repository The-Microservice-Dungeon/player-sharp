using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Events.Models.Map;
using Sharp.Player.Manager;

namespace Sharp.Player.Events.Consumers.Map;

public class GameworldCreatedMessageHandler : IMessageHandler<GameworldCreatedEvent>
{
    private readonly ILogger<GameworldCreatedMessageHandler> _logger;
    private readonly IMapManager _mapManager;

    public GameworldCreatedMessageHandler(IMapManager mapManager, ILogger<GameworldCreatedMessageHandler> logger)
    {
        _mapManager = mapManager;
        _logger = logger;
    }

    public Task Handle(IMessageContext context, GameworldCreatedEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);

        _mapManager.Create(message.Id);
        foreach (var messageSpacestationId in message.SpacestationIds)
            _mapManager.AddSpaceStation(messageSpacestationId);

        return Task.CompletedTask;
    }
}