namespace Sharp.Gameplay.Game;

public class CommandBuilderDirector
{
    private readonly string _gameId;
    private readonly string _playerToken;

    public CommandBuilderDirector(string gameId, string playerToken)
    {
        this._gameId = gameId;
        this._playerToken = playerToken;
    }

    public BuyCommandBuilder BuyCommand =>
        new BuyCommandBuilder(new BaseCommand(_gameId, _playerToken, CommandType.Buying));
}