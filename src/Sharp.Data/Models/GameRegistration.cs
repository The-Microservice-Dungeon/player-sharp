using System.ComponentModel.DataAnnotations;

namespace Sharp.Data.Model;

public class GameRegistration
{
    [Required]
    public string GameId { get; private set; }
    
    [Required]
    public string TransactionId { get; private set; }
    
    [Required]
    public PlayerDetails PlayerDetails { get; set; }

    public GameRegistration(string gameId, string transactionId)
    {
        GameId = gameId;
        TransactionId = transactionId;
    }
}