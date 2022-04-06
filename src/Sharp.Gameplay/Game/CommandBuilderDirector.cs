namespace Sharp.Gameplay.Game;

/// <summary>
///     Director to the fluent builders. Always associated to a single game and player
/// </summary>
public class CommandBuilderDirector
{
    private readonly string _gameId;
    private readonly string _playerToken;

    public CommandBuilderDirector(string gameId, string playerToken)
    {
        _gameId = gameId;
        _playerToken = playerToken;
    }
    
    public BuyCommandBuilder BuyCommand => new(new BaseCommand(_gameId, _playerToken, CommandType.Buying));
    public MovementCommandBuilder MovementCommand => new(new BaseCommand(_gameId, _playerToken, CommandType.Movement));
}