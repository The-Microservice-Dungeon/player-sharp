using System.Diagnostics;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Data.Context;
using Sharp.Data.Model;
using Sharp.Player.Consumers.Model;
using Sharp.Player.Manager;
using Sharp.Player.Services;

namespace Sharp.Player.Consumers;

public class PlayerStatusMessageHandler : IMessageHandler<PlayerStatusEvent>
{
    private readonly IPlayerManager _playerManager;
    private readonly SharpDbContext _db;

    public PlayerStatusMessageHandler(IPlayerManager playerManager, SharpDbContext db)
    {
        _playerManager = playerManager;
        _db = db;
    }

    public Task Handle(IMessageContext context, PlayerStatusEvent message)
    {
        var transactionId = context.Headers.GetString("transactionId") ?? throw new ApplicationException("There must be an transaction id");
        var details = _playerManager.ResolveRegistrationTransactionId(transactionId);
        if (details != null)
        {
            Debug.Assert(details.Name == message.Name);
            Debug.Assert(details.PlayerId == null || details.PlayerId == message.PlayerId);

            if (details.PlayerId != message.PlayerId)
            {
                _playerManager.SetPlayerId(message.PlayerId);
            }
        }

        return Task.CompletedTask;
    }
}