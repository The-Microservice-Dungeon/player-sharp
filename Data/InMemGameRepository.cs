using Player.Sharp.Consumers;

namespace Player.Sharp.Data
{
    public class InMemGameRepository : IGameRepository
    {
        private Game? _game;

        public void Save(Game game)
        {
            _game = game;
        }

        public Game Get()
        {
            if(_game == null)
                throw new ApplicationException("Game is null and shouldn't be");
            return _game;
        }

        public void Clear()
        {
            _game = null;
        }
    }
}
