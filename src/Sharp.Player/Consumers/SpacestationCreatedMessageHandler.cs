using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Gameplay.Map;
using Sharp.Player.Consumers.Model;
using Sharp.Player.Manager;

namespace Sharp.Player.Consumers;

public class SpacestationCreatedMessageHandler : IMessageHandler<SpacestationCreatedEvent>
{
    private readonly IMapManager _mapManager;

    public SpacestationCreatedMessageHandler(IMapManager mapManager)
    {
        _mapManager = mapManager;
    }

    public async Task Handle(IMessageContext context, SpacestationCreatedEvent message)
    {
        var map = _mapManager.Get();
        while (map == null)
        {
            map = _mapManager.Get();
            await Task.Delay(50);
        }
        var field = map.GetField(message.PlanetId) ?? new Field(message.PlanetId);
        field.SpaceStation = new SpaceStation(field);
    }
}