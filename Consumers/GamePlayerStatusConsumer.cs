using System.Text.Json.Serialization;
using Confluent.Kafka;

namespace Player.Sharp.Consumers;

public class GamePlayerStatusEvent
{
    [JsonPropertyName("playerId")] public string PlayerId { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }
}

public class GamePlayerStatusConsumer : DungeonEventConsumer<string, RoundStatusEvent>
{
    private readonly ILogger _logger;

    public GamePlayerStatusConsumer(IConfiguration config, ILogger<GamePlayerStatusConsumer> logger) : base(
        "playerStatus", config)
    {
        _logger = logger;
    }

    protected override void Consume(ConsumeResult<string, RoundStatusEvent> cr)
    {
        _logger.LogDebug($"{cr.Message.Key}: {cr.Message.Value}");
    }
}