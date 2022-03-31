using KafkaFlow;
using Sharp.Player.Config;
using Sharp.Player.Repository;

namespace Sharp.Player.Middleware.Kafka;

public class TransactionIdConsumptionMarker : IMessageMiddleware
{
    private readonly ITransactionContextStore _transactionContext;
    private readonly ILogger<TransactionIdConsumptionMarker> _logger;

    public TransactionIdConsumptionMarker(ITransactionContextStore transactionContext, ILogger<TransactionIdConsumptionMarker> logger)
    {
        _transactionContext = transactionContext;
        _logger = logger;
    }

    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        await next(context).ConfigureAwait(false);

        if (context.ConsumerContext.ShouldStoreOffset)
        {
            var transactionId = context.Headers.GetString(KafkaHeaders.TransactionIdHeaderName);
            if (transactionId == null)
            {
                _logger.LogDebug("No {Header} Header present", KafkaHeaders.TransactionIdHeaderName);
            }
            else
            {
                _transactionContext.MarkAsConsumed(transactionId, context.ConsumerContext.ConsumerName);
            }
        }
    }
}