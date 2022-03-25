﻿using AutoMapper;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Gameplay.Game;
using Sharp.Gameplay.Trading;

namespace Sharp.Player.Manager;

public class CommandManager : ICommandManager
{
    private readonly IGameCommandClient _commandClient;
    private readonly IGameManager _gameManager;
    private readonly ILogger<CommandManager> _logger;
    private readonly IMapper _mapper;
    private readonly IPlayerManager _playerManager;

    public CommandManager(ILogger<CommandManager> logger, IPlayerManager playerManager, IGameManager gameManager,
        IGameCommandClient commandClient, IMapper mapper)
    {
        _logger = logger;
        _playerManager = playerManager;
        _gameManager = gameManager;
        _commandClient = commandClient;
        _mapper = mapper;
    }

    // TODO: What is this
    public string GameId { get; set; }

    public CommandBuilderDirector CommandBuilder => new(GameId, _playerManager.Get().Token);

    public async Task BuyRobot(uint amount = 1)
    {
        if (amount == 0)
            throw new ArgumentException("Amount cannot be 0", nameof(amount));

        var command = CommandBuilder.BuyCommand
            .SetItem(Item.Robot)
            .SetQuantity(amount)
            .Build();
        var request = _mapper.Map<CommandRequest>(command);

        await _commandClient.SendCommand(request);
    }
}