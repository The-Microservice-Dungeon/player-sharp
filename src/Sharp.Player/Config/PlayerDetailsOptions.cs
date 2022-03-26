using System.ComponentModel.DataAnnotations;

namespace Sharp.Player.Config;

/// <summary>
///     Environment Options concerning the player
/// </summary>
public class PlayerDetailsOptions
{
    public const string PlayerDetails = "PlayerDetails";

    [Required] public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}