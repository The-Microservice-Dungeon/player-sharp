using System.Text.Json.Serialization;
using Confluent.Kafka;
using Player.Sharp.Services;

namespace Player.Sharp.Consumers;

public class RobotDestroyedEvent
{
    [JsonPropertyName("robotId")] public string RobotId { get; set; }

    [JsonPropertyName("playerId")] public string PlayerId { get; set; }
}

public class RobotDestroyedEventConsumer : DungeonEventConsumer<string, RobotDestroyedEvent>
{
    private readonly ILogger _logger;
    private readonly RobotService _robotService;

    public RobotDestroyedEventConsumer(IConfiguration config, ILogger<RobotDestroyedEventConsumer> logger,
        RobotService robotService) : base("robot-destroyed", config)
    {
        _logger = logger;
        _robotService = robotService;
    }

    protected override void Consume(ConsumeResult<string, RobotDestroyedEvent> cr)
    {
        var @event = cr.Message.Value;
        _robotService.RemoveRobot(@event.RobotId);
    }
}