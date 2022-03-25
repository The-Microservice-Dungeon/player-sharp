using System.Text;
using KafkaFlow;
using Microsoft.EntityFrameworkCore;
using Sharp.Data.Context;
using Sharp.Data.Model;

namespace Sharp.Player.Middleware.Kafka;

/// <summary>
///    Resolves a given Transaction Id in the header (if present) and attaches the corresponding Game ID to it. 
/// </summary>
public class TransactionIdResolver : IMessageMiddleware
{
    private readonly ILogger<TransactionIdResolver> _logger;
    private readonly DbSet<CommandTransaction> _commandTransactions;

    public TransactionIdResolver(ILogger<TransactionIdResolver> logger, SharpDbContext dbContext)
    {
        _logger = logger;
        _commandTransactions = dbContext.CommandTransactions;
    }

    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        var transactionId = context.Headers["transactionId"];
        if (transactionId == null)
        {
            _logger.LogDebug("No TransactionId Header present");
            await next(context).ConfigureAwait(false);
            return;
        }
        
        var id = Encoding.UTF8.GetString(transactionId);
        var commandTransaction = await _commandTransactions.FindAsync(id);
        if (commandTransaction == null)
        {
            _logger.LogDebug("No Entry for Transaction Id '{TId}' found. Probably it is for another player", id);
            await next(context).ConfigureAwait(false);
            return;
        }
        
        context.Headers.Add("GameId", Encoding.UTF8.GetBytes(commandTransaction.GameId));
        await next(context).ConfigureAwait(false);
    }
}