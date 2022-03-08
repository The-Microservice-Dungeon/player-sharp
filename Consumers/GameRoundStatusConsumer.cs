using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using System.Text.Json;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Player.Sharp.Consumers
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoundStatus { 
        [EnumMember(Value = "started")]
        STARTED,
        [EnumMember(Value = "command input ended")]
        COMMAND_INPUT_ENDED,
        [EnumMember(Value = "ended")]
        ENDED 
    }

    public class RoundStatusEvent
    {
        [JsonPropertyName("gameId")]
        public string GameId { get; set; }
        [JsonPropertyName("status")]
        public RoundStatus Status { get; set; }
        [JsonPropertyName("roundId")]
        public string RoundId { get; set; }
        [JsonPropertyName("roundNumber")]
        public uint RoundNumber { get; set; }
    }

    public class GameRoundStatusConsumer : DungeonEventConsumer<string, RoundStatusEvent>
    {
        private readonly ILogger _logger;
        public GameRoundStatusConsumer(IConfiguration config, ILogger<GameRoundStatusConsumer> logger) : base("roundStatus", config)
        {
            _logger = logger;
        }

        protected override void Consume(ConsumeResult<string, RoundStatusEvent> cr)
        {
            _logger.LogDebug($"{cr.Message.Key}: {cr.Message.Value}");
        }
    }
}
