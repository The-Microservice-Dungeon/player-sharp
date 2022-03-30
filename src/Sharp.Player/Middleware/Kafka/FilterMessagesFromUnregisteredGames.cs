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
    private readonly IServiceScopeFactory _scopeFactory;

    public FilterMessagesFromUnregisteredGames(ILogger<FilterMessagesFromUnregisteredGames> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        var gameIdHeader = context.Headers.GetString(KafkaHeaders.GameIdHeaderName);

        if (gameIdHeader == null)
        {
            _logger.LogWarning(
                $"No GameID Header was found, did you registered the {nameof(TransactionIdResolver)} first?");
            await next(context).ConfigureAwait(false);
            return;
        }

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SharpDbContext>();
            if (db.GameRegistrations.Any(r => r.GameId == gameIdHeader))
                await next(context).ConfigureAwait(false);

            _logger.LogDebug("Received a message in Topic '{Topic}' which does not belong to any registered game",
                context.ConsumerContext.Topic);
        }
    }
}