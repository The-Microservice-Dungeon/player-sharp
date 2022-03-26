﻿using System.ComponentModel.DataAnnotations;

namespace Sharp.Data.Models;

/// <summary>
///     Stores Transaction IDs which belong to performed commands.
/// </summary>
public class CommandTransaction
{
    /// <param name="gameId">The game ID of which the Transaction ID belongs to</param>
    /// <param name="transactionId">Transaction ID</param>
    public CommandTransaction(string gameId, string transactionId)
    {
        GameId = gameId;
        TransactionId = transactionId;
    }

    [Required] public string GameId { get; }

    [Required] public string TransactionId { get; }
}