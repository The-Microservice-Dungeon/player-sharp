using System.Diagnostics;
using KafkaFlow;
using Sharp.Player.Repository;

namespace Sharp.Player.Middleware.Kafka;

public class DeferUntilWalletIsCreated : IMessageMiddleware
{
    private readonly IWalletStore _walletStore;
    private readonly ILogger<DeferUntilWalletIsCreated> _logger;

    public DeferUntilWalletIsCreated(IWalletStore walletStore, ILogger<DeferUntilWalletIsCreated> logger)
    {
        _walletStore = walletStore;
        _logger = logger;
    }

    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        var timer = new Stopwatch();
        timer.Start();
        while (!_walletStore.IsSet())
        {
            if (timer.Elapsed.Minutes > 2)
                throw new Exception(
                    "After two minutes there is still no bank created. Have you restarted the player in a running game?");
        }
        timer.Stop();
        _logger.LogInformation("It took {Time}s to initialize the wallet", timer.Elapsed.TotalSeconds);
        await next(context).ConfigureAwait(false);
    }
}