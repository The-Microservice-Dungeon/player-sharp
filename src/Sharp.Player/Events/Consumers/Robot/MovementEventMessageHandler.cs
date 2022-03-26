using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Events.Models.Robot;
using Sharp.Player.Manager;

namespace Sharp.Player.Events.Consumers.Robot;

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