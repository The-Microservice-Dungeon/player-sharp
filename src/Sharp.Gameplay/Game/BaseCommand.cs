using System.Diagnostics.CodeAnalysis;

namespace Sharp.Gameplay.Game;

// TODO: We're not bound to this model and could get rid of the nullabilty shit by simply introducing our own command
//  model and map it onto a DTO.
public struct BaseCommand
{
    public readonly string GameId;
    public readonly string PlayerToken;
    public readonly CommandType CommandType;
    public BaseCommandObject CommandObject;

    internal BaseCommand(string gameId, string playerToken, CommandType commandType)
    {
        GameId = gameId;
        PlayerToken = playerToken;
        CommandType = commandType;
        CommandObject = new BaseCommandObject(commandType);
    }
    
    public string? RobotId { get; set; } = null;
}