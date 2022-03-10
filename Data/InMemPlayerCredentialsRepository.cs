using Player.Sharp.Consumers;

namespace Player.Sharp.Data;

public class InMemPlayerCredentialsRepository : IPlayerCredentialsRepository
{
    private PlayerCredentials? _credentials;

    public PlayerCredentials Get()
    {
        if (_credentials == null) throw new ApplicationException("Well that shouldn't be null");
        return _credentials;
    }

    public bool Exists()
    {
        return _credentials != null;
    }

    public void Save(PlayerCredentials playerCredentials)
    {
        _credentials = playerCredentials;
    }
}