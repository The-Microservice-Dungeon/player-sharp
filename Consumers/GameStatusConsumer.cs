using Confluent.Kafka;
using System.Text.Json;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Player.Sharp.Core
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GameStatus { 
        [EnumMember(Value = "created")]
        CREATED,
        [EnumMember(Value = "started")]
        STARTED,
        [EnumMember(Value = "ended")]
        ENDED 
    }

    public class GameStatusEvent
    {
        [JsonPropertyName("gameId")]
        public string GameId { get; set; }
        [JsonPropertyName("status")]
        public GameStatus Status { get; set; }
    }

    public class GameStatusConsumer : DungeonEventConsumer<string, GameStatusEvent>
    {
        private readonly ILogger _logger;
        public GameStatusConsumer(IConfiguration config, ILogger<GameStatusConsumer> logger) : base("status", config)
        {
            _logger = logger;
        }

        protected override void Consume(ConsumeResult<string, GameStatusEvent> cr)
        {
            Console.WriteLine($"{cr.Message.Key}: {cr.Message.Value}");
            _logger.LogDebug($"{cr.Message.Key}: {cr.Message.Value}");
        }
    }
}
