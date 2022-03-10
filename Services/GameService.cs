﻿using Player.Sharp.Client;
using Player.Sharp.Consumers;
using Player.Sharp.Core;
using Player.Sharp.Data;

namespace Player.Sharp.Services;

public class GameService
{
    private readonly IGameClient _gameClient;
    private readonly IGameRepository _gameRepository;
    private readonly ILogger<GameService> _logger;
    private readonly IPlayerCredentialsRepository _playerCredentialsRepository;

    public GameService(IGameClient gameClient,
        ILogger<GameService> logger,
        IGameRepository gameRepository,
        IPlayerCredentialsRepository playerCredentialsRepository)
    {
        _gameClient = gameClient;
        _logger = logger;
        _gameRepository = gameRepository;
        _playerCredentialsRepository = playerCredentialsRepository;
    }

    public async Task<Game> RegisterForGame(string gameId)
    {
        _logger.LogInformation("Try to register for Game with ID '{GameID}'", gameId);
        var credentials = _playerCredentialsRepository.Get();

        // Should be successful, otherwise fail-fast, I don't know how to recover from this
        await _gameClient.RegisterForGame(gameId, credentials.Token);

        var game = new Game(gameId);
        _gameRepository.Save(game);

        _logger.LogInformation("Successfully registered for Game with ID '{GameID}'", gameId);
        return game;
    }

    public void ForgetGame()
    {
        _gameRepository.Clear();
    }

    public bool GameIsRunning()
    {
        return _gameRepository.Exists();
    }

    public Game GetCurrentGame()
    {
        return _gameRepository.Get();
    }

    public async Task<string> SendCommand(Command command)
    {
        var response = await _gameClient.PostCommand(command);

        return response.TransactionId;
    }
}