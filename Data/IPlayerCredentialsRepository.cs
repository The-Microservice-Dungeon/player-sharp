using Player.Sharp.Core;

namespace Player.Sharp.Data
{
    public interface IPlayerCredentialsRepository
    {
        PlayerCredentials Get();
        bool Exists();
        void Save(PlayerCredentials playerCredentials);
    }
}
