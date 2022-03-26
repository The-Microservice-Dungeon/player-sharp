using System.Diagnostics;
using System.Globalization;
using KafkaFlow;
using Sharp.Player.Config;

namespace Sharp.Player.Middleware.Kafka;

/// <summary>
///     Discards respective commits messages that are older than a predefined thereshold.
/// </summary>
public class FilterOldMessages : IMessageMiddleware
{
    private const uint DivergenceLimit = 100000;

    private readonly ILogger<FilterOldMessages> _logger;

    // Thereshold in ms
    private readonly uint _thersehold = 300000;

    public FilterOldMessages(ILogger<FilterOldMessages> logger)
    {
        _logger = logger;
    }

    /// <param name="thersehold">Thereshold in ms</param>
    public FilterOldMessages(uint thersehold, ILogger<FilterOldMessages> logger)
    {
        _thersehold = thersehold;
        _logger = logger;
    }

    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        // Timestamp defined by our headers
        DateTime? dateTimeFromTimestampHeader = null;
        var timestampHeader = context.Headers.GetString(KafkaHeaders.TimestampHeaderName);

        if (timestampHeader != null)
            dateTimeFromTimestampHeader = DateTime.Parse(timestampHeader, null, DateTimeStyles.RoundtripKind);

        // Timestamp of the message
        var messageTimestamp = context.ConsumerContext.MessageTimestamp;

        // The following process determines the date attribute from the consumer record that should be used to check the
        // date. The process is mainly influenced by the uncertency and unreliablity of the services. 
        // We start using the message timestamp header
        var referenceTime = messageTimestamp;
        if (dateTimeFromTimestampHeader != null)
        {
            // If a timestamp header is present we use this header
            referenceTime = dateTimeFromTimestampHeader.Value;
            // But if it is diverged we fall back to the message timestamp
            if (Math.Abs(messageTimestamp.CompareTo(dateTimeFromTimestampHeader)) > DivergenceLimit)
            {
                _logger.LogWarning(
                    "The message Timestamp Value '{MessageTimestamp}' and Timestamp Header Value '{Timestamp}' are diverged. Falling back to the message timestamp",
                    messageTimestamp, dateTimeFromTimestampHeader);
                referenceTime = messageTimestamp;
            }
        }

        var diff = DateTime.Now - referenceTime;
        Debug.Assert(diff.Milliseconds >= 0);

        if (diff.Milliseconds >= _thersehold)
            return;

        await next(context).ConfigureAwait(false);
    }
}