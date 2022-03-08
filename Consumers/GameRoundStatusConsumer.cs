using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Player.Sharp.Consumers
{
    [JsonConverter(typeof(StringEnumConverter))]
    enum RoundStatus { 
        [EnumMember(Value = "started")]
        STARTED,
        [EnumMember(Value = "command input ended")]
        COMMAND_INPUT_ENDED,
        [EnumMember(Value = "ended")]
        ENDED 
    }

    class RoundStatusEvent
    {
        [JsonRequired]
        [JsonProperty("gameId")]
        public string GameId { get; set; }
        [JsonRequired]
        [JsonProperty("status")]
        public RoundStatus Status { get; set; }
        [JsonRequired]
        [JsonProperty("roundId")]
        public string RoundId { get; set; }
        [JsonRequired]
        [JsonProperty("roundNumber")]
        public string RoundNumber { get; set; }
    }

    public class RoundStatusConsumer : BackgroundService
    {
        private readonly string topic = "status";
        private readonly IConsumer<string, RoundStatusEvent> consumer;

        public RoundStatusConsumer(IConfiguration config)
        {
            var consumerConfig = new ConsumerConfig();
            config.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
            consumer = new ConsumerBuilder<string, RoundStatusEvent>(consumerConfig)
                .SetValueDeserializer(new JsonDeserializer<RoundStatusEvent>().AsSyncOverAsync())
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            new Thread(() => StartConsumerLoop(stoppingToken)).Start();

            return Task.CompletedTask;
        }

        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            consumer.Subscribe(this.topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = this.consumer.Consume(cancellationToken);

                    Console.WriteLine($"{cr.Message.Key}: {cr.Message.Value}");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    // Consumer errors should generally be ignored (or logged) unless fatal.
                    Console.WriteLine($"Consume error: {e.Error.Reason}");

                    if (e.Error.IsFatal)
                    {
                        // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e}");
                    break;
                }
            }
        }

        public override void Dispose()
        {
            this.consumer.Close(); // Commit offsets and leave the group cleanly.
            this.consumer.Dispose();

            base.Dispose();
        }
    }
}
