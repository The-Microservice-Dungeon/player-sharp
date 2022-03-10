using Player.Sharp.Core;

namespace Player.Sharp.Data;

public interface ITransactionRepository
{
    bool ExistsById(string id);
    void Save(Transaction transaction);
    void RemoveById(string id);
    void Clear();
}