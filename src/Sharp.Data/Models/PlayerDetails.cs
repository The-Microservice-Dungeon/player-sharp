using System.ComponentModel.DataAnnotations;

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

    [Required] public string Name { get; }

    [Required] public string Email { get; }

    [Required] public string Token { get; set; }

    public string? PlayerId { get; set; }
}