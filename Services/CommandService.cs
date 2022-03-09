using Player.Sharp.Client;
using Player.Sharp.Core;
using Player.Sharp.Data;

namespace Player.Sharp.Services
{
    
    public class CommandService
    {
        private readonly ILogger<CommandService> _logger;
        private readonly TransactionService _transactionService;
        private readonly IPlayerCredentialsRepository _playerCredentialsRepository;
        private readonly GameService _gameService;

        public CommandService(ILogger<CommandService> logger, 
            TransactionService transactionService, 
            IPlayerCredentialsRepository playerCredentialsRepository,
            GameService gameService)
        {
            _logger = logger;
            _transactionService = transactionService;
            _playerCredentialsRepository = playerCredentialsRepository;
            _gameService = gameService;
        }

        public async void BuyRobot(uint qty)
        {
            Command command = GetDefaultCommand("buying");
            command.CommandObject.ItemName = "ROBOT";
            command.CommandObject.ItemQty = qty;

            PerformCommand(command);
        }

        private Command GetDefaultCommand(string type)
        {
            Command command = new();
            command.GameId = _gameService.GetCurrentGame().ID;
            command.PlayerToken = _playerCredentialsRepository.Get().Token;
            command.CommandType = type;
            command.CommandObject.CommandType = type;
            return command;
        }

        private async void PerformCommand(Command command)
        {
            _logger.LogInformation("Performing Command: '{Command}'", command);
            var result = await _gameService.SendCommand(command);
            _transactionService.SaveTransactionId(result);
            _logger.LogInformation("Command was received by the Game Service. Transaction ID: '{TransactionId}'", result);
        }
    }
}
