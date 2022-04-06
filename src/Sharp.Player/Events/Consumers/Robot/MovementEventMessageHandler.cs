using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Gameplay.Map;
using Sharp.Player.Config;
using Sharp.Player.Events.Models.Robot;
using Sharp.Player.Manager;
using Sharp.Player.Repository;

namespace Sharp.Player.Events.Consumers.Robot;

public class MovementEventMessageHandler : IMessageHandler<MovementEvent>
{
    private readonly ILogger<MovementEventMessageHandler> _logger;
    private readonly IMapManager _mapManager;
    private readonly IRobotManager _robotManager;
    private readonly ITransactionContextStore _transactionContextStore;

    public MovementEventMessageHandler(IMapManager mapManager, ILogger<MovementEventMessageHandler> logger, IRobotManager robotManager, ITransactionContextStore transactionContextStore)
    {
        _mapManager = mapManager;
        _logger = logger;
        _robotManager = robotManager;
        _transactionContextStore = transactionContextStore;
    }

    public Task Handle(IMessageContext context, MovementEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);

        if (!message.Success)
        {
            _logger.LogError("Received unsuccessful Movement Event: {Message}", message);
            return Task.CompletedTask;
        }
        
        var planet = message.Planet!;
        switch (planet.PlanetType)
        {
            case PlanetType.Default:
                List<ResourceType> resources = new();
                ResourceType? resourceType = planet.ResourceType;
                if (resourceType != null)
                {
                    resources.Add(resourceType.Value);
                } 
                _mapManager.AddPlanet(planet.PlanetId, planet.MovementDifficulty, resources);
                break;
            case PlanetType.Spacestation:
                _mapManager.AddSpaceStation(planet.PlanetId);
                break;
        }
        
        var robotId = context.Headers.GetString(KafkaHeaders.RobotIdHeaderName);
        if (robotId == null)
            throw new Exception("Robot is not present");
        
        _robotManager.MoveRobot(robotId, planet.PlanetId);
        
        var transactionId = context.Headers.GetString(KafkaHeaders.TransactionIdHeaderName);
        if (transactionId == null)
            throw new Exception("TransactionId is not present");
        _transactionContextStore.AddContext(transactionId, ContextKeys.PlanetId, planet.PlanetId);

        return Task.CompletedTask;
    }
}