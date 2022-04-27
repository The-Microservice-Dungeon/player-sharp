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

    private BaseCommand BuildBaseCommand(CommandType type) => new(_gameId, _playerToken, type);
    
    public BuyCommandBuilder BuyCommand => new(BuildBaseCommand(CommandType.Buying));
    public MovementCommandBuilder MovementCommand => new(BuildBaseCommand(CommandType.Movement));

    public RegenerateCommandBuilder RegenerateCommand => new(BuildBaseCommand(CommandType.Regeneration));
}