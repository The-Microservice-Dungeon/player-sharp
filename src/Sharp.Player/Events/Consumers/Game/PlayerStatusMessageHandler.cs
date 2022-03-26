using System.Diagnostics;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Data.Contexts;
using Sharp.Player.Events.Models.Game;
using Sharp.Player.Manager;

namespace Sharp.Player.Events.Consumers.Game;

public class PlayerStatusMessageHandler : IMessageHandler<PlayerStatusEvent>
{
    private readonly SharpDbContext _db;
    private readonly IPlayerManager _playerManager;

    public PlayerStatusMessageHandler(IPlayerManager playerManager, SharpDbContext db)
    {
        _playerManager = playerManager;
        _db = db;
    }

    public Task Handle(IMessageContext context, PlayerStatusEvent message)
    {
        var transactionId = context.Headers.GetString("transactionId") ??
                            throw new ApplicationException("There must be an transaction id");
        var details = _playerManager.ResolveRegistrationTransactionId(transactionId);
        if (details != null)
        {
            Debug.Assert(details.Name == message.Name);
            Debug.Assert(details.PlayerId == null || details.PlayerId == message.PlayerId);

            if (details.PlayerId != message.PlayerId) _playerManager.SetPlayerId(message.PlayerId);
        }

        return Task.CompletedTask;
    }
}