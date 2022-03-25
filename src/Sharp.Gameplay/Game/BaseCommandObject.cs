using Sharp.Gameplay.Trading;

namespace Sharp.Gameplay.Game;

public struct BaseCommandObject
{
    public readonly CommandType CommandType;
    public string? PlanetId = null;
    public string? TargetId = null;
    public Item? ItemName = null;
    public uint? ItemQuantity = null;

    public BaseCommandObject(CommandType commandType)
    {
        CommandType = commandType;
    }
}