using Sharp.Client.Client;
using Sharp.Data.Contexts;

namespace Sharp.Player.Services;

public class CommandTransactionCleanupService : BackgroundService
{
    private readonly SharpDbContext _dbContext;
    private readonly ILogger<CommandTransactionCleanupService> _logger;
    private readonly IGameClient _gameClient;

    public CommandTransactionCleanupService(SharpDbContext dbContext, ILogger<CommandTransactionCleanupService> logger, IGameClient gameClient)
    {
        _dbContext = dbContext;
        _logger = logger;
        _gameClient = gameClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var activeGameIds = (await _gameClient.GetAllActiveGames()).Select(g => g.GameId);
            var danglingCommandTransactions = _dbContext.CommandTransactions.Where(transaction => activeGameIds.All(id => id != transaction.GameId))
                .ToList();
            _dbContext.RemoveRange(danglingCommandTransactions);
            await _dbContext.SaveChangesAsync(stoppingToken);
        }
        catch (Exception e)
        {
            // We don't want the service to crash. Since it is not as cruicial to clean up the database. 
            _logger.LogWarning(e, "Could not clean up command transactions");
        }
    }
}