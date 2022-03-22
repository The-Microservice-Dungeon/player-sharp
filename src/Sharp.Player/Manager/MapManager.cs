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

        var field = _map.GetField(fieldId) ?? new Field(fieldId);

        if (field.SpaceStation != null)
            return;

        field.SetSpaceStation(new SpaceStation());
        _map.AddField(field);

        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.FieldUpdated(field).GetAwaiter().GetResult();
    }
}