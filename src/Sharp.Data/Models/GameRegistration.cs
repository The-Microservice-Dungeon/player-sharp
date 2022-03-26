using System.ComponentModel.DataAnnotations;

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

    [Required] public string GameId { get; }

    [Required] public string TransactionId { get; }

    [Required] public PlayerDetails PlayerDetails { get; set; }
}