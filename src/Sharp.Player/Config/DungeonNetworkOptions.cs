using System.ComponentModel.DataAnnotations;

namespace Sharp.Player.Config;

public class DungeonNetworkOptions
{
    public const string DungeonNetwork = "DungeonNetwork";

    [Required] 
    public string KafkaAddress { get; set; }
    
    [Required]
    public string GameServiceAddress { get; set; }
}