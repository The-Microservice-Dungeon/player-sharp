using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using System.Text.Json;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Player.Sharp.Gameplay;
using Player.Sharp.Services;

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
        private readonly IEnumerable<IRoundLifecycleHandler> _roundLifecycleHandlers;
        private readonly GameService _gameService;
        public GameRoundStatusConsumer(IConfiguration config, 
            ILogger<GameRoundStatusConsumer> logger, 
            IEnumerable<IRoundLifecycleHandler> roundLifecycleHandlers,
            GameService gameService) : base("roundStatus", config)
        {
            _logger = logger;
            _roundLifecycleHandlers = roundLifecycleHandlers;
            _gameService = gameService;
        }

        protected override void Consume(ConsumeResult<string, RoundStatusEvent> cr)
        {
            var @event = cr.Message.Value;

            if (_gameService.GameIsRunning() && _gameService.GetCurrentGame().ID == @event.GameId)
            {
                // I like this approach where the strategies are components themself. However, many improvements are necessary
                // - Priority to select the best applicable strategy
                // - Execute all Handlers instead of just the first one
                // - More that I forgot because I'm braindead
                var handler = _roundLifecycleHandlers.Where(handler => handler.CheckCondition())
                    .First();

                if (@event.Status == RoundStatus.STARTED)
                {
                    handler.OnRoundStart();
                }
                else if (@event.Status == RoundStatus.ENDED)
                {
                    handler.OnRoundEnd();
                }
            }
        }
    }
}
