using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace Player.Sharp.Consumers
{
    public class JsonMessageSerde<V> : IDeserializer<V>, ISerializer<V> where V : class
    {
        public V Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if(isNull)
            {
                throw new ApplicationException("Shouldn't be null");
            }
            var dataAsString = Encoding.UTF8.GetString(data);
            var result = JsonSerializer.Deserialize<V>(dataAsString);
            if (result == null)
            {
                throw new ApplicationException("Shouldn't be null");
            }
            return result!;
        }

        public byte[] Serialize(V data, SerializationContext context)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize<V>(data));
        }
    }

    public abstract class DungeonEventConsumer<K, V> : IHostedService where V : class
    {
        private readonly string topic;
        private readonly IConsumer<K, V> consumer;
        protected static IConsumer<K, V> BuildConsumer(IConfiguration config)
        {
            var consumerConfig = new ConsumerConfig();
            consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
            config.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
            return new ConsumerBuilder<K, V>(consumerConfig)
                .SetValueDeserializer(new JsonMessageSerde<V>())
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build();
        }

        public DungeonEventConsumer(string topic, IConfiguration config)
        {
            this.topic = topic;
            this.consumer = BuildConsumer(config);
        }

        protected abstract void Consume(ConsumeResult<K, V> cr);

        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            using(consumer)
            {
                Console.WriteLine($"Subscribing to {this.topic}");
                consumer.Subscribe(this.topic);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = this.consumer.Consume(cancellationToken);
                        this.Consume(cr);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (ConsumeException e)
                    {
                        // Consumer errors should generally be ignored (or logged) unless fatal.
                        Console.WriteLine($"Consume error: {e.Error.Reason}");
                        Console.WriteLine(e.ToString());

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
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            new Thread(() => StartConsumerLoop(cancellationToken)).Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
