using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Data.Context;
using Sharp.Data.Model;
using Sharp.Gameplay.Game;
using Sharp.Gameplay.Trading;

namespace Sharp.Player.Manager;

public class CommandManager : ICommandManager
{
    private readonly IGameCommandClient _commandClient;
    private readonly IGameManager _gameManager;
    private readonly ILogger<CommandManager> _logger;
    private readonly IMapper _mapper;
    private readonly IPlayerManager _playerManager;
    private readonly SharpDbContext _dbContext;

    public CommandManager(ILogger<CommandManager> logger, IPlayerManager playerManager, IGameManager gameManager,
        IGameCommandClient commandClient, IMapper mapper, SharpDbContext dbContext)
    {
        _logger = logger;
        _playerManager = playerManager;
        _gameManager = gameManager;
        _commandClient = commandClient;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    // TODO: What is this
    public string GameId { get; set; }

    public CommandBuilderDirector CommandBuilder => new(GameId, _playerManager.Get().Token);

    public async Task BuyRobot(uint amount = 1)
    {
        if (amount == 0)
            throw new ArgumentException("Amount cannot be 0", nameof(amount));

        var command = CommandBuilder.BuyCommand
            .SetItem(Item.Robot)
            .SetQuantity(amount)
            .Build();
        var request = _mapper.Map<CommandRequest>(command);

        var response = await _commandClient.SendCommand(request);
        await SaveCommandTransaction(GameId, response.TransactionId);
    }

    private async Task SaveCommandTransaction(string gameId, string transactionId)
    {
        _dbContext.CommandTransactions.Add(new CommandTransaction(gameId, transactionId));
        await _dbContext.SaveChangesAsync();
    } 
}