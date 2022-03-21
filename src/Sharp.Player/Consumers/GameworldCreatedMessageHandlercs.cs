using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Consumers.Model;
using Sharp.Player.Manager;

namespace Sharp.Player.Consumers;

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
        _logger.LogDebug(
            "Received Gameworld Created event with GameworldId {GameworldId} and SpaceStation IDs {SpaceStationIds}",
            message.Id, message.SpacestationIds);
        _mapManager.Create(message.Id);
        foreach (var messageSpacestationId in message.SpacestationIds)
            _mapManager.AddSpaceStation(messageSpacestationId);

        return Task.CompletedTask;
    }
}