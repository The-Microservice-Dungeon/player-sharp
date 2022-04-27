namespace Sharp.Player.Store;

/*
 * We could also directly interact with kafka. Check consumption with the admin client, etc. pp. But as we as a player
 * should not write to kafka, we stick to this hacky approach.
 */
public class MemoryTransactionContextStore : ITransactionContextStore
{
    private readonly Dictionary<string, HashSet<string>> _store = new();
    private readonly Dictionary<(string, string), List<byte[]>> _contextStore = new();

    private readonly ILogger<MemoryTransactionContextStore> _logger;

    public MemoryTransactionContextStore(ILogger<MemoryTransactionContextStore> logger)
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
        var cKey = (transactionId, key);
        if (!_contextStore.ContainsKey(cKey))
        {
            _contextStore.Add(cKey, new List<byte[]>());
        }

        _contextStore[cKey].Add(context);
    }

    public List<byte[]> GetContext(string transactionId, string key) => _contextStore[(transactionId, key)];

    public void RemoveContext(string transactionId, string key) => _contextStore.Remove((transactionId, key));

    public void ClearContext(string transactionId)
    {
        foreach (var keyValuePair in _contextStore.Where(c => c.Key.Item1 == transactionId))
        {
            _contextStore.Remove(keyValuePair.Key);
        }
    }
    public void ClearContext() => _contextStore.Clear();
}