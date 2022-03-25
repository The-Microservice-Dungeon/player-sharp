using System.ComponentModel.DataAnnotations;

namespace Sharp.Data.Model;

public class CommandTransaction
{
    [Required]
    public string GameId { get; private set; }
    
    [Required]
    public string TransactionId { get; private set; }

    public CommandTransaction(string gameId, string transactionId)
    {
        GameId = gameId;
        TransactionId = transactionId;
    }
}