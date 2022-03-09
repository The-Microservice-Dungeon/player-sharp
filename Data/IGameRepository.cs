using Player.Sharp.Core;

namespace Player.Sharp.Data
{
    public interface IGameRepository
    {
        Game FindById(string id);
        IEnumerable<Game> FindAll();
        void Save(Game game);
        void RemoveById(string id);
    }
}
