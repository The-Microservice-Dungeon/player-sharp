using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sharp.Data.Models;

/// <summary>
///     Stores the player details like name, email and token.
/// </summary>
public class PlayerDetails
{
    public PlayerDetails(string name, string email, string token)
    {
        Name = name;
        Email = email;
        Token = token;
    }

    [Key] [Column(Order = 0)] [Required] public string Name { get; set; }

    [Key] [Column(Order = 1)] [Required] public string Email { get; set; }

    [Required] public string Token { get; set; }

    public string? PlayerId { get; set; }
}