using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sharp.Gameplay.Map;
using Sharp.Player.Manager;

namespace Sharp.Player.Controllers;

[ApiController]
[Route("map")]
public class MapController : ControllerBase
{
    private IMapper _mapper;
    private IMapManager _mapManager;

    public MapController(IMapper mapper, IMapManager mapManager)
    {
        _mapper = mapper;
        _mapManager = mapManager;
    }

    [HttpGet]
    public ActionResult<MapDto> GetMap()
    {
        var map = _mapManager.Get();
        if (map == null)
            return NotFound();
        // TODO: The Mapping should be part of the Map Manager.
        return Ok(_mapper.Map<MapDto>(map));
    }
}

public class MapDto
{
    public string Id { get; }
    public Dictionary<string, FieldDto> Fields { get; }

    public MapDto(string id, Dictionary<string, FieldDto> fields)
    {
        Id = id;
        Fields = fields;
    }
}

public class FieldDto
{
    public PlanetDto? Planet { get; set; }
    public SpacestationDto? Spacestation { get; set; }
    public string[] Connections { get; }

    public FieldDto(string[] connections)
    {
        Connections = connections;
    }
}

public class PlanetDto
{
    
}

public class SpacestationDto
{
    
}