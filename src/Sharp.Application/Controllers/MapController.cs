using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sharp.Domain.Map;
using Sharp.Player.Store;

namespace Sharp.Player.Controllers;

[ApiController]
[Route("map")]
public class MapController : ControllerBase
{
    private readonly ICurrentMapStore _map;
    private readonly IMapper _mapper;

    public MapController(IMapper mapper, ICurrentMapStore map)
    {
        _mapper = mapper;
        _map = map;
    }

    [HttpGet]
    public ActionResult<MapDto> GetMap()
    {
        try
        {
            var map = _map.Get();
            return Ok(_mapper.Map<MapDto>(map));
        }
        catch (UnsetStateException)
        {
            return NotFound();
        }
    }
}

public class MapDto
{
    public MapDto(string id, Dictionary<string, FieldDto> fields)
    {
        Id = id;
        Fields = fields;
    }

    public string Id { get; }
    public Dictionary<string, FieldDto> Fields { get; }
}

public class FieldDto
{
    public FieldDto(string[] connections)
    {
        Connections = connections;
    }

    public int? MovementDifficulty { get; set; }
    public PlanetDto? Planet { get; set; }
    public SpacestationDto? Spacestation { get; set; }
    public string[] Connections { get; set; }
}

public class PlanetDto
{
    public ResourceDeposit[] ResourceDeposits { get; set; }
}

public class ResourceDepositDto
{
    public ResourceType ResourceType { get; set; }
}

public class SpacestationDto
{
}