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
    private readonly SharpDbContext _db;
    private readonly ILogger<CommandManager> _logger;
    private readonly IMapper _mapper;
    private readonly IPlayerDetailsProvider _playerDetails;
    private readonly IRobotManager _robotManager;
    private readonly IMapManager _mapManager;

    public CommandManager(ILogger<CommandManager> logger,
        IGameCommandClient commandClient, IMapper mapper, SharpDbContext db, IPlayerDetailsProvider playerDetails, IRobotManager robotManager, IMapManager mapManager)
    {
        _logger = logger;
        _commandClient = commandClient;
        _mapper = mapper;
        _db = db;
        _playerDetails = playerDetails;
        _robotManager = robotManager;
        _mapManager = mapManager;
    }

    // TODO: See ICommandManager
    public string GameId { get; set; }

    public CommandBuilderDirector CommandBuilder => new(GameId, _playerDetails.Get().Token);

    public async Task BuyRobot(uint amount = 1)
    {
        if (amount == 0)
            throw new ArgumentException("Amount cannot be 0", nameof(amount));
        if (amount > 1)
            throw new ArgumentException(
                "At the moment no more than 1 robot can be bought at a time because of problems with the location",
                nameof(amount));

        var command = CommandBuilder.BuyCommand
            .SetItem(Item.Robot)
            .SetQuantity(amount)
            .Build();

        await SendCommand(command);
    }

    // TODO: Just for testing
    public async Task RandomMovement()
    {
        var robot = _robotManager.GetRobots()[0];
        var neighbours = robot.Field.GetNeighbours();
        var random = new Random();
        var randomNeighbour = neighbours[random.Next(0, neighbours.Length)];
        var randomMovementCommand = CommandBuilder.MovementCommand
            .SetRobotId(robot.Id)
            .SetPlanetId(randomNeighbour.Id)
            .Build();

        await SendCommand(randomMovementCommand);
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
        _logger.LogDebug(
            "Save Command Transaction to {GameId} with Transaction ID {TransactionId} for Command {@Command}", gameId,
            transactionId, command);

        await _db.CommandTransactions.AddAsync(new CommandTransaction(gameId, transactionId)
        {
            PlanetId = command.CommandObject.PlanetId,
            RobotId = command.RobotId,
            TargetId = command.CommandObject.TargetId,
            CommandType = command.CommandType
        });
        await _db.SaveChangesAsync();
    }
}