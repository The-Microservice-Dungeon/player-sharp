using KafkaFlow;
using Sharp.Player.Events.Models.Trading;

namespace Sharp.Player.Events.TypeResolver.Trading;

public class TradeEventTypeResolver : IMessageTypeResolver
{
    private const string TypeHeader = "type";

    public Type OnConsume(IMessageContext context)
    {
        var typeName = context.Headers.GetString(TypeHeader);

        return typeName switch
        {
            "buy-error" => typeof(TradeErrorEvent),
            "buy-robot" => typeof(TradeBuyRobotEvent),
            "sell-resource" => typeof(TradeSellResourcesEvent),
            _ => Type.GetType(typeName) ?? typeof(string)
        };
    }

    public void OnProduce(IMessageContext context)
    {
        context.Headers.SetString(TypeHeader,
            $"{context.Message.GetType().FullName}, {context.Message.GetType().Assembly.GetName().Name}");
    }
}