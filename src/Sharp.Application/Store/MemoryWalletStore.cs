using Sharp.Domain.Trading;

namespace Sharp.Player.Store;

public class MemoryWalletStore : IWalletStore
{
    private Wallet? _wallet;

    public Wallet Get() => _wallet ?? throw new UnsetStateException("Wallet is not set");

    public void Set(Wallet data)
    {
        _wallet = data;
    }

    public bool IsSet() => _wallet != null;
}