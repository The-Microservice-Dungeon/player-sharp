using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Player.Consumers.Model;

namespace Sharp.Player.Consumers;

public class PlayerStatusMessageHandler : IMessageHandler<PlayerStatusEvent>
{
    public Task Handle(IMessageContext context, PlayerStatusEvent message)
    {
        throw new NotImplementedException();
    }
}