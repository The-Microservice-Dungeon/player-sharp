using System.Text;

namespace Sharp.Player.Repository;

/* Dev Comment:
 * As we're having some dependencies between different messages that are spread between several kafka topics we,
 * need to ensure the order. In order to ensure the order (nice wording) we need to mark messages as consumed and
 * check whether messages are consumed in order to defer message consumptiun.
 *
 * Take the following example.
 * - When a Robot spawns we get a message with its position (Let's call it "robot-spawned")
 * - When a Robot spawns we get a message with its positional neighbours ("neighbours")
 *
 * IF we consume the neighbours event first we don't know the releation of the neighbours. We need to ensure the
 * consumption of "robot-spawend" before "neighbours".
 * The cleanest possible solution would be a Kafka stream processor that take care of that. However, the player
 * shouldn't perform write operations in kafka, therefore we're going with a store-based approach.
 *
 * Also we might beed to attach some context for a specific message. For example it would ease the process of adding
 * neighbours if we know which position the neighbours-event would refer to. This is also done with this store.
 *
 * All in all this some kind of hacky approach to intercept technical debts. IT SHOULD BE REMOVED AND IS HEAVILY
 * ERRORNOUS TODO TODO TODO TODO
 */
public interface ITransactionIdContextStore
{
    bool HasBeenConsumed(string transactionId, string consumerName);
    void MarkAsConsumed(string transactionId, string consumerName);

    void AddContext(string transactionId, string key, string context)
    {
        AddContext(transactionId, key, Encoding.UTF8.GetBytes(context));
    }

    void AddContext(string transactionId, string key, byte[] context);

    List<string> GetContextAsString(string transactionId, string key)
    {
        return GetContext(transactionId, key)
            .Select(b => Encoding.UTF8.GetString(b))
            .ToList();
    }

    List<byte[]> GetContext(string transactionId, string key);
    void RemoveContext(string transactionId, string key);
    void ClearContext(string transactionId);
}

public class ContextKeys
{
    public const string PlanetId = "PlanetId";
    public const string RobotId = "RobotId";
}