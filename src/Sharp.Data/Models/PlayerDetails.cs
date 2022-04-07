using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sharp.Data.Models;

/// <summary>
///     Stores the player details like name, email and token.
/// </summary>
public class PlayerDetails
{
    public PlayerDetails(string playerId, string name, string email, string token)
    {
        PlayerId = playerId;
        Name = name;
        Email = email;
        Token = token;
    }

    [Required] public string Name { get; set; }

    [Required] public string Email { get; set; }

    [Required] public string Token { get; set; }

    [Key] [Required] public string PlayerId { get; set; }
}