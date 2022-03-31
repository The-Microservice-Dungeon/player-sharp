using KafkaFlow;
using Sharp.Player.Config;
using Sharp.Player.Repository;

namespace Sharp.Player.Middleware.Kafka;

public class TransactionIdConsumptionMarker : IMessageMiddleware
{
    private readonly ITransactionIdContextStore _transactionIdContext;
    private readonly ILogger<TransactionIdConsumptionMarker> _logger;

    public TransactionIdConsumptionMarker(ITransactionIdContextStore transactionIdContext, ILogger<TransactionIdConsumptionMarker> logger)
    {
        _transactionIdContext = transactionIdContext;
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
                _transactionIdContext.MarkAsConsumed(transactionId, context.ConsumerContext.ConsumerName);
            }
        }
    }
}