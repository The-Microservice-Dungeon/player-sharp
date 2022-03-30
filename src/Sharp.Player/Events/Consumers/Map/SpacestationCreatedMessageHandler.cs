using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Events.Models.Map;
using Sharp.Player.Manager;
using Sharp.Player.Repository;

namespace Sharp.Player.Events.Consumers.Map;

public class SpacestationCreatedMessageHandler : IMessageHandler<SpacestationCreatedEvent>
{
    private readonly ILogger<SpacestationCreatedMessageHandler> _logger;
    private readonly IMapManager _mapManager;
    private readonly ICurrentMapStore _mapStore;

    public SpacestationCreatedMessageHandler(ILogger<SpacestationCreatedMessageHandler> logger,
        ICurrentMapStore mapStore, IMapManager mapManager)
    {
        _logger = logger;
        _mapStore = mapStore;
        _mapManager = mapManager;
    }

    public async Task Handle(IMessageContext context, SpacestationCreatedEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", typeof(Message).FullName, message);

        // The Event is produced in a separate topic and COULD (in my tests it is most likely) that the spacestation-created
        //  event is even consumed before the gameworld-created event. Therefore we must handle the case where the map
        //  is unitialized. For sake of simplicity we just wait until it is intialized
        while (!_mapStore.IsSet()) await Task.Delay(50);
        var map = _mapStore.Get();

        _mapManager.AddSpaceStation(message.PlanetId);
    }
}