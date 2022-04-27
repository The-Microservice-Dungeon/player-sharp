using System.Text;
using KafkaFlow;
using Sharp.Infrastructure.Persistence.Contexts;
using Sharp.Player.Config;

namespace Sharp.Player.Middleware.Kafka;

/// <summary>
///     Resolves a given Transaction Id in the header (if present) and attaches the corresponding Game ID to it.
/// </summary>
public class TransactionIdResolver : IMessageMiddleware
{
    private readonly SharpDbContext _db;
    private readonly ILogger<TransactionIdResolver> _logger;

    public TransactionIdResolver(ILogger<TransactionIdResolver> logger, SharpDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        var transactionId = context.Headers.GetString(KafkaHeaders.TransactionIdHeaderName);
        if (transactionId == null)
        {
            _logger.LogDebug("No {Header} Header present", KafkaHeaders.TransactionIdHeaderName);
            await next(context).ConfigureAwait(false);
            return;
        }

        var commandTransaction = await _db.CommandTransactions.FindAsync(transactionId);

        if (commandTransaction == null)
        {
            _logger.LogDebug("No Entry for Transaction Id '{TId}' found. Probably it is for another player",
                transactionId);
            await next(context).ConfigureAwait(false);
            return;
        }

        context.Headers.Add(KafkaHeaders.GameIdHeaderName, Encoding.UTF8.GetBytes(commandTransaction.GameId));
        context.Headers.Add(KafkaHeaders.CommandTypeHeaderName,
            BitConverter.GetBytes((int)commandTransaction.CommandType));

        if (commandTransaction.PlanetId != null)
            context.Headers.Add(KafkaHeaders.PlanetIdHeaderName, Encoding.UTF8.GetBytes(commandTransaction.PlanetId));
        if (commandTransaction.RobotId != null)
            context.Headers.Add(KafkaHeaders.RobotIdHeaderName, Encoding.UTF8.GetBytes(commandTransaction.RobotId));
        if (commandTransaction.TargetId != null)
            context.Headers.Add(KafkaHeaders.TargetIdHeaderName, Encoding.UTF8.GetBytes(commandTransaction.TargetId));

        await next(context).ConfigureAwait(false);
    }
}