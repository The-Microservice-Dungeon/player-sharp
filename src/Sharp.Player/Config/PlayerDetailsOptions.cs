using System.ComponentModel.DataAnnotations;

namespace Sharp.Player.Config;

public class PlayerDetailsOptions
{
    public const string PlayerDetails = "PlayerDetails";

    [Required] public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}