using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Player.Sharp.Consumers
{
    class GamePlayerStatusEvent
    {
        [JsonRequired]
        [JsonProperty("playerId")]
        public string PlayerId { get; set; }
        [JsonRequired]
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class GamePlayerStatusConsumer : BackgroundService
    {
        private readonly string topic = "playerStatus";
        private readonly IConsumer<string, GamePlayerStatusEvent> consumer;

        public GamePlayerStatusConsumer(IConfiguration config)
        {
            var consumerConfig = new ConsumerConfig();
            config.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
            consumer = new ConsumerBuilder<string, GamePlayerStatusEvent>(consumerConfig)
                .SetValueDeserializer(new JsonDeserializer<GamePlayerStatusEvent>().AsSyncOverAsync())
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
