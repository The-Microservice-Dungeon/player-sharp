using AutoMapper;
using Sharp.Domain.Game;
using Sharp.Domain.Trading;
using Sharp.Infrastructure.Network.Client;
using Sharp.Infrastructure.Network.Model;
using Sharp.Infrastructure.Persistence.Contexts;
using Sharp.Infrastructure.Persistence.Models;
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
        
        // TODO: Regeneration does not belong here. Doing it just for testing purposes
        var needsRegeneration = robot.Attributes.Energy < robot.Field.MovementDifficulty;

        if (needsRegeneration)
        {
            var regenerationCommand = CommandBuilder.RegenerateCommand
                .SetRobotId(robot.Id)
                .Build();
            await SendCommand(regenerationCommand);
            return;
        }
            
        var neighbours = robot.Field.GetNeighbours();
        var random = new Random();
        // It can occur that we didn't have the neighbours yet while issuing a command. Not good this. 
        if (neighbours.Length > 0)
        {
            var randomNeighbour = neighbours[random.Next(0, neighbours.Length)];
            var randomMovementCommand = CommandBuilder.MovementCommand
                .SetRobotId(robot.Id)
                .SetPlanetId(randomNeighbour.Id)
                .Build();

            await SendCommand(randomMovementCommand);
        }
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