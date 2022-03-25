using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Consumers.Model;
using Sharp.Player.Manager;

namespace Sharp.Player.Consumers;

public class MovementEventMessageHandler : IMessageHandler<MovementEvent>
{
    private readonly ILogger<MovementEventMessageHandler> _logger;
    private readonly IMapManager _mapManager;

    public MovementEventMessageHandler(IMapManager mapManager, ILogger<MovementEventMessageHandler> logger)
    {
        _mapManager = mapManager;
        _logger = logger;
    }

    public Task Handle(IMessageContext context, MovementEvent message)
    {
        _logger.LogDebug(
            "Received Movement event: {Event}", message);

        if (message.Success)
        {
            var planet = message.Planet!;
            if (planet.PlanetType == PlanetType.DEFAULT)
            {
                _mapManager.AddPlanet(planet.PlanetId, planet.MovementDifficulty, new []{ planet.ResourceType });
            }
        }

        return Task.CompletedTask;
    }
}