using KafkaFlow;
using KafkaFlow.TypedHandler;
using Sharp.Client.Model;
using Sharp.Player.Events.Models.Game;
using Sharp.Player.Manager;

namespace Sharp.Player.Events.Consumers.Game;

public class GameStatusMessageHandler : IMessageHandler<GameStatusEvent>
{
    private readonly IGameManager _gameManager;
    private readonly IPlayerManager _playerManager;

    public GameStatusMessageHandler(IGameManager gameManager, IPlayerManager playerManager)
    {
        _gameManager = gameManager;
        _playerManager = playerManager;
    }

    public async Task Handle(IMessageContext context, GameStatusEvent message)
    {
        if (message.Status == GameStatus.Created)
        {
            await _gameManager.PerformRegistration(message.GameId, _playerManager.Get());
        }
    }
    
    
}