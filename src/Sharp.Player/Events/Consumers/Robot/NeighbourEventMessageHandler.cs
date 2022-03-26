using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Consumers.Model;
using Sharp.Player.Manager;

namespace Sharp.Player.Consumers;

public class NeighbourEventMessageHandler : IMessageHandler<NeighboursEvent>
{
    private readonly ILogger<NeighbourEventMessageHandler> _logger;
    private readonly IMapManager _mapManager;

    public NeighbourEventMessageHandler(IMapManager mapManager, ILogger<NeighbourEventMessageHandler> logger)
    {
        _mapManager = mapManager;
        _logger = logger;
    }

    public Task Handle(IMessageContext context, NeighboursEvent message)
    {
        _logger.LogDebug(
            "Received Neighbours event: {Event}", message);
        
        foreach (var neighbour in message.Neighbours)
        {
            _mapManager.AddOpaqueField(neighbour.PlanetId, neighbour.MovementDifficulty);
        }

        return Task.CompletedTask;
    }
}