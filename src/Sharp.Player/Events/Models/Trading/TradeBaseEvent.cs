using System.Text.Json.Serialization;

namespace Sharp.Player.Events.Models.Trading;

public abstract class TradeBaseEvent<T>
{
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("moneyChangedBy")] public int MoneyChangeBy { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }

    [JsonPropertyName("data")] public T Data { get; set; }
}