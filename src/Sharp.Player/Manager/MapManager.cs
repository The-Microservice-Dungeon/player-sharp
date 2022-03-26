using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Sharp.Gameplay.Map;
using Sharp.Player.Controllers;
using Sharp.Player.Hubs;

namespace Sharp.Player.Manager;

public class MapManager : IMapManager
{
    private readonly IHubContext<MapHub, IMapHub> _mapHubContext;
    private readonly ILogger<MapManager> _logger;
    public MapManager(IHubContext<MapHub, IMapHub> mapHubContext, ILogger<MapManager> logger)
    {
        _mapHubContext = mapHubContext;
        _logger = logger;
    }

    // TODO: Get rid of that nullability. 
    private Map? _map { get; set; }

    public Map? Get() => _map;
    public Map GetOrThrow() => _map ?? throw new ApplicationException($"{nameof(_map)} is null");
    

    public void Create(string id)
    {
        _logger.LogDebug("Creating a new Map with ID {Id}", id);
        
        _map = new Map(id);
        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.MapCreated(_map).GetAwaiter().GetResult();
    }

    public void AddSpaceStation(string fieldId)
    {
        _logger.LogDebug("Adding SpaceStation on Field {Id}", fieldId);
        
        var field = GetOrThrow().GetOrCreateField(fieldId);

        if (field.SpaceStation != null)
            return;

        var spacestation = new SpaceStation();
        field.SetSpaceStation(spacestation);
        
        _logger.LogDebug("SpaceStation {SpaceStation} added", spacestation);

        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.FieldUpdated(field).GetAwaiter().GetResult();
    }
    
    public void AddOpaqueField(string id, int movementDifficulty)
    {
        _logger.LogDebug("Adding Opaque field with {Id}", id);
        
        var field = GetOrThrow().GetOrCreateField(id);
        if (field.MovementDifficulty == movementDifficulty)
            return;

        field.MovementDifficulty = movementDifficulty;
        
        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.FieldUpdated(field).GetAwaiter().GetResult();
    }

    public void AddPlanet(string id, int movementDifficulty, ResourceType[] resourceTypes)
    {
        _logger.LogDebug("Adding Planet on Field {Id}", id);
        
        var field = GetOrThrow().GetOrCreateField(id);
        if(field.MovementDifficulty == movementDifficulty && 
           field.Planet != null && 
           field.Planet.ResourceDeposits.Select(d => d.ResourceType).SequenceEqual(resourceTypes))
            return;

        field.MovementDifficulty = movementDifficulty;
        var planet = new Planet
        {
            ResourceDeposits = resourceTypes.Select(type => new ResourceDeposit(type)).ToArray()
        };
        field.SetPlanet(planet);
        
        _logger.LogDebug("Planet {Planet} added", planet);
        
        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.FieldUpdated(field).GetAwaiter().GetResult();
    }

    public Field GetField(string id) => GetOrThrow().GetOrCreateField(id);
}