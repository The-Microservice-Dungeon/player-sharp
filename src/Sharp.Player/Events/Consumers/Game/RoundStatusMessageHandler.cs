using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Consumers.Model;
using Sharp.Player.Manager;

namespace Sharp.Player.Consumers;

public class RoundStatusMessageHandler : IMessageHandler<RoundStatusEvent>
{
    private readonly ILogger<RoundStatusMessageHandler> _logger;
    private readonly ICommandManager _commandManager;

    public RoundStatusMessageHandler(ILogger<RoundStatusMessageHandler> logger, ICommandManager commandManager)
    {
        _logger = logger;
        _commandManager = commandManager;
    }

    public async Task Handle(IMessageContext context, RoundStatusEvent message)
    {
        _logger.LogDebug(
            "Received Round Status event: {Event}", message);

        _commandManager.GameId = message.GameId;
        if (message.RoundStatus == RoundStatus.Started)
        {
            await _commandManager.BuyRobot();
        }
    }
}