using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Gameplay.Map;
using Sharp.Player.Consumers.Model;
using Sharp.Player.Manager;

namespace Sharp.Player.Consumers;

public class GameworldCreatedMessageHandler : IMessageHandler<GameworldCreatedEvent>
{
    private IMapManager _mapManager;

    public GameworldCreatedMessageHandler(IMapManager mapManager)
    {
        _mapManager = mapManager;
    }

    public Task Handle(IMessageContext context, GameworldCreatedEvent message)
    {
        var map = _mapManager.Create(message.Id);
        // TODO: This should probably be in the manager
        foreach (var messageSpacestationId in message.SpacestationIds)
        {
            var field = new Field(messageSpacestationId);
            field.SpaceStation = new SpaceStation(field);
            map.AddField(field);
        }
        
        return Task.CompletedTask;
    }
}