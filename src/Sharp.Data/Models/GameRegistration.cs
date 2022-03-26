using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sharp.Data.Models;

/// <summary>
///     Stores game registrations.
/// </summary>
public class GameRegistration
{
    public GameRegistration(string gameId, string transactionId)
    {
        GameId = gameId;
        TransactionId = transactionId;
    }

    [Key]
    [Required] public string GameId { get; set; }

    [Required] public string TransactionId { get; set; }
    
    public string PlayerName { get; set; }
    public string PlayerEmail { get; set; }
    
    [Required] 
    [ForeignKey("PlayerName, PlayerEmail")]
    public virtual PlayerDetails PlayerDetails { get; set; }
}