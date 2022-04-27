using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Domain.Game;
using Sharp.Player.Config;
using Sharp.Player.Events.Models.Robot;
using Sharp.Player.Manager;
using Sharp.Player.Store;

namespace Sharp.Player.Events.Consumers.Robot;

public class NeighbourEventMessageHandler : IMessageHandler<NeighboursEvent>
{
    private readonly ILogger<NeighbourEventMessageHandler> _logger;
    private readonly IMapManager _mapManager;
    private readonly ITransactionContextStore _transactionContext;

    public NeighbourEventMessageHandler(IMapManager mapManager, ILogger<NeighbourEventMessageHandler> logger,
        ITransactionContextStore transactionContext)
    {
        _mapManager = mapManager;
        _logger = logger;
        _transactionContext = transactionContext;
    }

    public async Task Handle(IMessageContext context, NeighboursEvent message)
    {
        _logger.LogDebug("Received {Event} Message {@Message}", message.GetType().FullName, message);
        var commandType = (CommandType)BitConverter.ToInt32(context.Headers[KafkaHeaders.CommandTypeHeaderName]);

        if (commandType is not (CommandType.Buying or CommandType.Movement))
            throw new Exception($"Invalid command type: ${commandType}");
        
        var transactionId = context.Headers.GetString(KafkaHeaders.TransactionIdHeaderName);
        if (transactionId == null)
            throw new Exception("TransactionId is not present");

        // A robot is being bought. Otherwise the movement does not make any sense
        if (commandType == CommandType.Buying)
        {
            // Defer until robot has been added to fleet / positioned on the map
            // We now, that a robot buy event will result into a robot id set in the transaction context
            // This assumption allows us to defer the message until this id is being set.
            for (var i = 0;
                 _transactionContext.GetContextAsStringOrDefault(transactionId, ContextKeys.RobotId) == null;
                 i++)
            {
                if (i == 10)
                    throw new Exception("Waiting for Robot ID took more than 10 iterations");
                // Ahhh we're blocking the consumer...
                await Task.Delay(100);
            }
        }
        else
        {
            // Defer until robot movement has been consumed
            // We now, that a robot movement event will result into a planet id set in the transaction context
            // This assumption allows us to defer the message until this id is being set.
            for (var i = 0;
                 _transactionContext.GetContextAsStringOrDefault(transactionId, ContextKeys.PlanetId) == null;
                 i++)
            {
                if (i == 10)
                    throw new Exception("Waiting for Planet ID took more than 10 iterations");
                // And blocking again... 
                await Task.Delay(100);
            }
        }
            
        // Now we know that the event is consumed and we can add the neighbours to the planet
        var planetIds = _transactionContext.GetContextAsString(transactionId, ContextKeys.PlanetId);
        if (planetIds.Count != 1)
            throw new Exception($"Unexpected Planet count. Found {planetIds.Count} planets");
        var planetId = planetIds[0];
        foreach (var neighbour in message.Neighbours)
            _mapManager.AddNeighbour(planetId, neighbour.PlanetId, neighbour.MovementDifficulty);
    }
}