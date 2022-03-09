using Player.Sharp.Consumers;

namespace Player.Sharp.Data
{
    public interface IGameRepository
    {
        Game Get();
        void Save(Game game);
        void Clear();
    }
}
