using Player.Sharp.Core;

namespace Player.Sharp.Data;

public class InMemTransactionRepository : ITransactionRepository
{
    private readonly Dictionary<string, Transaction> _storage = new();

    public bool ExistsById(string id)
    {
        return _storage.ContainsKey(id);
    }

    public void RemoveById(string id)
    {
        _storage.Remove(id);
    }

    public void Save(Transaction transaction)
    {
        _storage.Add(transaction.ID, transaction);
    }

    public void Clear()
    {
        _storage.Clear();
    }
}