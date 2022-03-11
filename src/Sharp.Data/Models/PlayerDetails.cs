using System.ComponentModel.DataAnnotations;

namespace Sharp.Data.Model;

public class PlayerDetails
{
    [Required]
    public string Name { get; private set; }
    [Required]
    public string Email { get; private set; }
    [Required]
    public string Token { get; set; }

    public PlayerDetails(string name, string email, string token)
    {
        Name = name;
        Email = email;
        Token = token;
    }
}