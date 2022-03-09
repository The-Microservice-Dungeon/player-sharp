using Confluent.Kafka;
using Player.Sharp.Consumers;
using Player.Sharp.Services;
using System.Text;
using System.Text.Json.Serialization;

namespace Player.Sharp.Consumers
{
    public class GameWorldCreatedEvent
    {
        [JsonPropertyName("id")]
        public string GameworldId { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("spacestation_ids")]
        public string[] SpacestationIds { get; set; }
    }

    public class MapGameworldCreatedConsumer : DungeonEventConsumer<string, GameWorldCreatedEvent>
    {
        private readonly ILogger _logger;
        private readonly MapService _mapService;
        public MapGameworldCreatedConsumer(IConfiguration config, 
            ILogger<GameWorldCreatedEvent> logger,
            MapService mapService) : base("gameworld-created", config)
        {
            _logger = logger;
            _mapService = mapService;
        }

        protected override void Consume(ConsumeResult<string, GameWorldCreatedEvent> cr)
        {
            var @event = cr.Message.Value;
            _logger.LogInformation("A new Gameworld was created. Spacestations can be found at: '{Spacestations}'", String.Join(", ", @event.SpacestationIds));
            _mapService.CreateMap(@event.GameworldId, @event.SpacestationIds);
        }
    }
}
