using System.ComponentModel.DataAnnotations;

namespace Sharp.Player.Config;

/// <summary>
///     Environment Options concerning the dungeon network architecture.
/// </summary>
public class DungeonNetworkOptions
{
    public const string DungeonNetwork = "DungeonNetwork";

    [Required] public string KafkaAddress { get; set; }

    [Required] public string GameServiceAddress { get; set; }
}