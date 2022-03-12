using KafkaFlow;
using Sharp.Player.Consumers.Model;

namespace Sharp.Player.Consumers;

public class DungeonMessageTypeResolver : IMessageTypeResolver
{
    private const string TypeHeader = "type";
    
    public Type OnConsume(IMessageContext context)
    {
        var typeName = context.Headers.GetString(TypeHeader);

        if (typeName == null)
            return typeof(string);

        return typeName switch
        {
            "player-status" => typeof(PlayerStatusEvent),
            _ => Type.GetType(typeName) ?? typeof(string)
        };
    }

    public void OnProduce(IMessageContext context)
    {
        context.Headers.SetString(TypeHeader, $"{context.Message.GetType().FullName}, {context.Message.GetType().Assembly.GetName().Name}");
    }
}