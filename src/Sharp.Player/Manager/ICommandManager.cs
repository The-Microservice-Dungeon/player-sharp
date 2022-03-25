using Sharp.Gameplay.Game;

namespace Sharp.Player.Manager;

public interface ICommandManager
{
    public string GameId { get; set; }
    public CommandBuilderDirector CommandBuilder { get; }
    public Task BuyRobot(uint amount = 1);
}