using AutoMapper;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Data.Contexts;
using Sharp.Data.Models;
using Sharp.Gameplay.Game;
using Sharp.Gameplay.Trading;
using Sharp.Player.Provider;

namespace Sharp.Player.Manager;

public class CommandManager : ICommandManager
{
    private readonly IGameCommandClient _commandClient;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IGameManager _gameManager;
    private readonly ILogger<CommandManager> _logger;
    private readonly IMapper _mapper;
    private readonly IPlayerManager _playerManager;

    public CommandManager(ILogger<CommandManager> logger, IPlayerManager playerManager, IGameManager gameManager,
        IGameCommandClient commandClient, IMapper mapper, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _playerManager = playerManager;
        _gameManager = gameManager;
        _commandClient = commandClient;
        _mapper = mapper;
        _scopeFactory = scopeFactory;
    }

    // TODO: See ICommandManager
    public string GameId { get; set; }

    public CommandBuilderDirector CommandBuilder
    {
        get
        {
            using var scope = _scopeFactory.CreateScope();
            var playerDetailsProvider = scope.ServiceProvider.GetRequiredService<IPlayerDetailsProvider>();
            return new(GameId, playerDetailsProvider.Get().Token);
        }
    }

    public async Task BuyRobot(uint amount = 1)
    {
        if (amount == 0)
            throw new ArgumentException("Amount cannot be 0", nameof(amount));

        var command = CommandBuilder.BuyCommand
            .SetItem(Item.Robot)
            .SetQuantity(amount)
            .Build();

        await SendCommand(command);
    }

    private async Task SendCommand(BaseCommand command)
    {
        _logger.LogDebug("Sending command {@Command}", command);
        
        var request = _mapper.Map<CommandRequest>(command);
        var response = await _commandClient.SendCommand(request);
        await SaveCommandTransaction(GameId, response.TransactionId, command);
    }

    private async Task SaveCommandTransaction(string gameId, string transactionId, BaseCommand command)
    {
        _logger.LogDebug("Save Command Transaction to {GameId} with Transaction ID {TransactionId} for Command {@Command}", gameId, transactionId, command);

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SharpDbContext>();
        await db.CommandTransactions.AddAsync(new CommandTransaction(gameId, transactionId)
        {
            PlanetId = command.CommandObject.PlanetId,
            RobotId = command.RobotId,
            TargetId = command.CommandObject.TargetId
        });
        await db.SaveChangesAsync();
    }
}