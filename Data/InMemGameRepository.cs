using Player.Sharp.Core;

namespace Player.Sharp.Data
{
    public class InMemGameRepository : IGameRepository
    {
        private readonly Dictionary<string, Game> _storage = new();

        public IEnumerable<Game> FindAll()
        {
            return _storage.Values;
        }

        public Game FindById(string id)
        {
            return _storage[id];
        }

        public void Save(Game game)
        {
            _storage.Add(game.ID, game);
        }

        public void RemoveById(string id)
        {
            _storage.Remove(id);
        }
    }
}
