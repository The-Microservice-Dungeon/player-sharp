using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace Player.Sharp.Consumers;

public class JsonMessageSerde<V> : IDeserializer<V>, ISerializer<V> where V : class
{
    public V Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull) throw new ApplicationException("Shouldn't be null");
        var dataAsString = Encoding.UTF8.GetString(data);
        var result = JsonSerializer.Deserialize<V>(dataAsString);
        if (result == null) throw new ApplicationException("Shouldn't be null");
        return result!;
    }

    public byte[] Serialize(V data, SerializationContext context)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
    }
}

public abstract class DungeonEventConsumer<K, V> : IHostedService where V : class
{
    private readonly IConsumer<K, V> consumer;
    private readonly string topic;

    public DungeonEventConsumer(string topic, IConfiguration config)
    {
        this.topic = topic;
        consumer = BuildConsumer(config);
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

    protected abstract void Consume(ConsumeResult<K, V> cr);

    private void StartConsumerLoop(CancellationToken cancellationToken)
    {
        using (consumer)
        {
            Console.WriteLine($"Subscribing to {topic}");
            consumer.Subscribe(topic);

            while (!cancellationToken.IsCancellationRequested)
                try
                {
                    var cr = consumer.Consume(cancellationToken);
                    Consume(cr);
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
                        // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                        break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e}");
                    break;
                }
        }
    }
}