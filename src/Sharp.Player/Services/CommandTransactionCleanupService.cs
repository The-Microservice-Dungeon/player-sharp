using Sharp.Client.Client;
using Sharp.Data.Contexts;

namespace Sharp.Player.Services;

public class CommandTransactionCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CommandTransactionCleanupService> _logger;
    private readonly IGameClient _gameClient;

    public CommandTransactionCleanupService(IServiceScopeFactory scopeFactory, ILogger<CommandTransactionCleanupService> logger, IGameClient gameClient)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _gameClient = gameClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Started {Service}", nameof(CommandTransactionCleanupService));
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SharpDbContext>();
            var activeGameIds = (await _gameClient.GetAllActiveGames()).Select(g => g.GameId);
            var danglingCommandTransactions = db.CommandTransactions
                .Where(transaction => activeGameIds.All(id => id != transaction.GameId))
                .ToList();

            if (danglingCommandTransactions.Count > 0)
            {
                _logger.LogDebug("There are {N} dangling command transactions that will be removed", danglingCommandTransactions.Count);
                db.RemoveRange(danglingCommandTransactions);
                await db.SaveChangesAsync(stoppingToken);
            }
        }
        catch (Exception e)
        {
            // We don't want the service to crash. Since it is not as cruicial to clean up the database. 
            _logger.LogWarning(e, "Could not clean up command transactions");
        }
    }
}