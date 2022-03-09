using Player.Sharp.Client;
using Player.Sharp.Data;
using Player.Sharp.Consumers;

namespace Player.Sharp.Services
{
    public class GameService
    {
        private readonly IGameClient _gameClient;
        private readonly ILogger<GameService> _logger;
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerCredentialsRepository _playerCredentialsRepository;

        public GameService(IGameClient gameClient, 
            ILogger<GameService> logger,
            IGameRepository gameRepository,
            IPlayerCredentialsRepository playerCredentialsRepository)
        {
            _gameClient = gameClient;
            _logger = logger;
            _gameRepository = gameRepository;
            _playerCredentialsRepository = playerCredentialsRepository;
        }
        
        public async Task<Game> RegisterForGame(string gameId)
        {
            _logger.LogInformation("Try to register for Game with ID '{GameID}'", gameId);
            var credentials = _playerCredentialsRepository.Get();

            // Should be successful, otherwise fail-fast, I don't know how to recover from this
            await _gameClient.RegisterForGame(gameId, credentials.Token);

            var game = new Game(gameId);
            _gameRepository.Save(game);

            _logger.LogInformation("Successfully registered for Game with ID '{GameID}'", gameId);
            return game;
        }

        public void ForgetGame(string gameId)
        {
            _gameRepository.Clear();
        }
    }
}
