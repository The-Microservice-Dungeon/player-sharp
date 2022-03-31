namespace Sharp.Player.Repository;

/*
 * We could also directly interact with kafka. Check consumption with the admin client, etc. pp. But as we as a player
 * should not write to kafka, we stick to this hacky approach.
 */
public class MemoryTransactionIdContextStore : ITransactionIdContextStore
{
    private readonly Dictionary<string, HashSet<string>> _store = new();
    private readonly Dictionary<string, Dictionary<string, List<byte[]>>> _contextStore = new();

    private readonly ILogger<MemoryTransactionIdContextStore> _logger;

    public MemoryTransactionIdContextStore(ILogger<MemoryTransactionIdContextStore> logger)
    {
        _logger = logger;
    }

    public bool HasBeenConsumed(string transactionId, string consumerName) => _store.GetValueOrDefault(transactionId)?
        .Any(consumer => consumer == consumerName) ?? false;

    public void MarkAsConsumed(string transactionId, string consumerName)
    {
        _logger.LogDebug("Marking {TransactionId} For Consumer {Consumer} as consumed", transactionId, consumerName);
        if (_store.ContainsKey(transactionId))
        {
            _store[transactionId].Add(consumerName);
        }
        else
        {
            HashSet<string> set = new() { consumerName };
            _store.Add(transactionId, set);
        }
    }

    public void AddContext(string transactionId, string key, byte[] context)
    {
        _logger.LogDebug("Adding Context For {TransactionId} with Key {Key} and context length {Length}", transactionId, key, context.Length);
        if (!_contextStore.ContainsKey(transactionId))
        {
            Dictionary<string, List<byte[]>> tStore = new();
            _contextStore.Add(transactionId, tStore);
        }
        else if (!_contextStore[transactionId].ContainsKey(key))
        {
            _contextStore[transactionId].Add(key, new List<byte[]>());
        }

        _contextStore[transactionId][key].Add(context);
    }

    public List<byte[]> GetContext(string transactionId, string key) => _contextStore[transactionId][key];

    public void RemoveContext(string transactionId, string key) => _contextStore[transactionId].Remove(key);

    public void ClearContext(string transactionId) => _contextStore[transactionId].Clear();
}