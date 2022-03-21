using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Consumers.Model;
using Sharp.Player.Manager;

namespace Sharp.Player.Consumers;

public class SpacestationCreatedMessageHandler : IMessageHandler<SpacestationCreatedEvent>
{
    private readonly ILogger<SpacestationCreatedMessageHandler> _logger;
    private readonly IMapManager _mapManager;

    public SpacestationCreatedMessageHandler(IMapManager mapManager, ILogger<SpacestationCreatedMessageHandler> logger)
    {
        _mapManager = mapManager;
        _logger = logger;
    }

    // TODO: Place mor of that in the Map Manager or maybe use a FieldManager?
    public async Task Handle(IMessageContext context, SpacestationCreatedEvent message)
    {
        _logger.LogDebug("Received SpacestationCreatedEvent event with PlanetId {PlanetId}", message.PlanetId);
        var map = _mapManager.Get();
        // The Event is produced in a separate topic and COULD (in my tests it is most likely) that the spacestation-created
        //  event is even consumed before the gameworld-created event. Therefore we must handle the case where the map
        //  is unitialized. For sake of simplicity we just wait until it is intialized
        while (map == null)
        {
            map = _mapManager.Get();
            await Task.Delay(50);
        }

        _mapManager.AddSpaceStation(message.PlanetId);
    }
}