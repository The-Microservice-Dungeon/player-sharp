using Confluent.Kafka;
using Player.Sharp.Services;
using System.Text;
using System.Text.Json.Serialization;

namespace Player.Sharp.Consumers
{
    public class TradeEvent<T> where T : class
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("moneyChangeBy")]
        public int MoneyChangeBy { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }

    public class BuyRobotData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
    public class TradingTradesBuyRobotEventConsumer : DungeonEventConsumer<string, TradeEvent<BuyRobotData>>
    {
        private readonly ILogger _logger;
        private readonly TransactionService _transactionService;
        private readonly RobotService _robotService;

        public TradingTradesBuyRobotEventConsumer(IConfiguration configuration, 
            RobotService robotService, 
            TransactionService transactionService, 
            ILogger<TradingTradesBuyRobotEventConsumer> logger) : base("trades", configuration)
        {
            _robotService = robotService;
            _transactionService = transactionService;
            _logger = logger;
        }

        protected override void Consume(ConsumeResult<string, TradeEvent<BuyRobotData>> cr)
        {
            var type = Encoding.UTF8.GetString(cr.Message.Headers.Where(header => header.Key == "type").First().GetValueBytes());
            var transactionId = Encoding.UTF8.GetString(cr.Message.Headers.Where(header => header.Key == "transactionId").First().GetValueBytes());
            if (type == "buy-robot" && _transactionService.IsMyTransactionId(transactionId))
            {
                var @event = cr.Message.Value;
                if (@event.Success == true)
                {
                    _logger.LogInformation("Successfuly Bought a robot");
                    _robotService.AddRobot(@event.Data.Id);
                } else
                {
                    _logger.LogInformation("Robot couldn't be bought. Message: {Message}", @event.Message);
                }
            }
        }
    }
}
