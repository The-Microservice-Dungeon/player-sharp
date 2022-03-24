using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sharp.Player.Manager;

namespace Sharp.Player.Controllers;

[ApiController]
[Route("games")]
public class GameController : ControllerBase
{
    private readonly IGameManager _gameManager;
    private readonly IMapper _mapper;

    public GameController(IMapper mapper, IGameManager gameManager)
    {
        _mapper = mapper;
        _gameManager = gameManager;
    }

    [HttpGet]
    public async Task<ActionResult<List<GameDto>>> GetGames()
    {
        var games = await _gameManager.GetRegisteredGames();
        return Ok(games.Select(game => _mapper.Map<GameDto>(game)).ToList());
    }
}

public class GameDto
{
    public string Id { get; set; }
} 