namespace Sharp.Domain.Game;

/// <summary>
///     Fluent API Builder for a Movement command
/// </summary>
public class MovementCommandBuilder : CommandBuilderFacade
{
    public MovementCommandBuilder(BaseCommand command) : base(command)
    {
    }

    public MovementCommandBuilder SetRobotId(string robotId)
    {
        Command.RobotId = robotId;
        return this;
    }

    public MovementCommandBuilder SetPlanetId(string planetId)
    {
        Command.CommandObject.PlanetId = planetId;
        return this;
    }

    protected override bool IsValid()
    {
        return Command.RobotId != null && Command.CommandObject.PlanetId != null;
    }
}