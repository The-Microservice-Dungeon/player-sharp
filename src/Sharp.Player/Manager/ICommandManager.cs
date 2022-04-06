using Sharp.Gameplay.Game;

namespace Sharp.Player.Manager;

public interface ICommandManager
{
    // TODO: Is this really necessary
    /// <summary>
    ///     Because a command is always associated with a game it is necessary to have an internal state in the
    ///     command manager.
    /// </summary>
    public string GameId { get; set; }

    public CommandBuilderDirector CommandBuilder { get; }
    public Task BuyRobot(uint amount = 1);
    // TODO: Just for testing
    public Task RandomMovement();
}