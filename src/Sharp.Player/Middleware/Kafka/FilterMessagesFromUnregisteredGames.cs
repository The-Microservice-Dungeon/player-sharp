using System.Diagnostics;
using System.Globalization;
using System.Text;
using KafkaFlow;
using Sharp.Data.Contexts;
using Sharp.Player.Config;

namespace Sharp.Player.Middleware.Kafka;

/// <summary>
///     Discards messages that belong to a game where the player is not registered.
/// </summary>
public class FilterMessagesFromUnregisteredGames : IMessageMiddleware
{
    private readonly ILogger<FilterMessagesFromUnregisteredGames> _logger;
    private readonly SharpDbContext _dbContext;

    public FilterMessagesFromUnregisteredGames(ILogger<FilterMessagesFromUnregisteredGames> logger, SharpDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        var gameIdHeader = context.Headers.GetString(KafkaHeaders.GameIdHeaderName);

        if (gameIdHeader == null)
            throw new Exception(
                $"No GameID Header was found, did you registered the {nameof(TransactionIdResolver)} first?"); 

        if(_dbContext.GameRegistrations.Any(r => r.GameId == gameIdHeader))
            await next(context).ConfigureAwait(false);
        
        _logger.LogDebug("Received a message in Topic '{Topic}' which does not belong to any registered game", context.ConsumerContext.Topic);
    }
}