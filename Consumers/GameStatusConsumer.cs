﻿using Confluent.Kafka;
using System.Text.Json;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Player.Sharp.Services;

namespace Player.Sharp.Core
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GameStatus { 
        [EnumMember(Value = "created")]
        CREATED,
        [EnumMember(Value = "started")]
        STARTED,
        [EnumMember(Value = "ended")]
        ENDED 
    }

    public class GameStatusEvent
    {
        [JsonPropertyName("gameId")]
        public string GameId { get; set; }
        [JsonPropertyName("status")]
        public GameStatus Status { get; set; }
    }

    public class GameStatusConsumer : DungeonEventConsumer<string, GameStatusEvent>
    {
        private readonly ILogger _logger;
        private readonly GameService _gameService;
        public GameStatusConsumer(IConfiguration config, ILogger<GameStatusConsumer> logger, GameService gameService) : base("status", config)
        {
            _logger = logger;
            _gameService = gameService;
        }

        protected override async void Consume(ConsumeResult<string, GameStatusEvent> cr)
        {
            var gameEvent = cr.Message.Value;
            
            if (gameEvent.Status == GameStatus.CREATED)
            {
                _logger.LogInformation("A new game with ID '{GameID}' has been created", gameEvent.GameId);
                await _gameService.RegisterForGame(gameEvent.GameId);
            }
            else if (gameEvent.Status == GameStatus.ENDED)
            {
                _logger.LogInformation("The game with ID '{GameID}' has been ended", gameEvent.GameId);
                _gameService.ForgetGame(gameEvent.GameId);
            }
            else if (gameEvent.Status == GameStatus.STARTED)
            {
                _logger.LogInformation("The game with ID '{GameID}' has started", gameEvent.GameId);
                // I think the event itself is unecessary for the player implementation as we will get everything
                // necessary from the round status events. 
            }
        }
    }
}
