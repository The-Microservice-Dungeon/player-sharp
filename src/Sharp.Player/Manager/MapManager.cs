using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Sharp.Gameplay.Map;
using Sharp.Player.Controllers;
using Sharp.Player.Hubs;

namespace Sharp.Player.Manager;

public class MapManager : IMapManager
{
    private readonly IHubContext<MapHub, IMapHub> _mapHubContext;
    public MapManager(IHubContext<MapHub, IMapHub> mapHubContext)
    {
        _mapHubContext = mapHubContext;
    }

    // TODO: Get rid of that nullability. 
    private Map? _map { get; set; }

    public Map? Get()
    {
        return _map;
    }

    public void Create(string id)
    {
        _map = new Map(id);
        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.MapCreated(_map).GetAwaiter().GetResult();
    }

    public void AddSpaceStation(string fieldId)
    {
        if (_map == null)
            // Shouldn't happen
            throw new ApplicationException("Map is null.");

        var field = _map.GetOrCreateField(fieldId);

        if (field.SpaceStation != null)
            return;

        field.SetSpaceStation(new SpaceStation());

        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.FieldUpdated(field).GetAwaiter().GetResult();
    }
    
    public void AddOpaqueField(string id, int movementDifficulty)
    {
        if (_map == null)
            // Shouldn't happen
            throw new ApplicationException("Map is null.");
        
        var field = _map.GetOrCreateField(id);
        if (field.MovementDifficulty == movementDifficulty)
            return;

        field.MovementDifficulty = movementDifficulty;
        
        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.FieldUpdated(field).GetAwaiter().GetResult();
    }

    public void AddPlanet(string id, int movementDifficulty, ResourceType[] resourceTypes)
    {
        if (_map == null)
            // Shouldn't happen
            throw new ApplicationException("Map is null.");
        
        var field = _map.GetOrCreateField(id);
        if(field.MovementDifficulty == movementDifficulty && 
           field.Planet != null && 
           field.Planet.ResourceDeposits.Select(d => d.ResourceType).SequenceEqual(resourceTypes))
            return;

        field.MovementDifficulty = movementDifficulty;
        field.SetPlanet(new Planet
        {
            ResourceDeposits = resourceTypes.Select(type => new ResourceDeposit(type)).ToArray()
        }); 
        
        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.FieldUpdated(field).GetAwaiter().GetResult();
    }
}