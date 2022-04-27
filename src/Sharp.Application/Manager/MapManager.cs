using Microsoft.AspNetCore.SignalR;
using Sharp.Domain.Map;
using Sharp.Player.Hubs;

namespace Sharp.Player.Manager;

public class MapManager : IMapManager
{
    private readonly ICurrentMapStore _currentMapStore;
    private readonly ILogger<MapManager> _logger;
    private readonly IHubContext<MapHub, IMapHub> _mapHubContext;

    public MapManager(IHubContext<MapHub, IMapHub> mapHubContext, ILogger<MapManager> logger,
        ICurrentMapStore currentMapStore)
    {
        _mapHubContext = mapHubContext;
        _logger = logger;
        _currentMapStore = currentMapStore;
    }

    public void Create(string id)
    {
        _logger.LogDebug("Creating a new Map with ID {Id}", id);

        var map = new Map(id);
        _currentMapStore.Set(map);

        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.MapCreated(map).GetAwaiter().GetResult();
    }

    public void AddSpaceStation(string fieldId)
    {
        _logger.LogDebug("Adding SpaceStation on Field {Id}", fieldId);

        var field = _currentMapStore.Get().GetOrCreateField(fieldId);

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

        var field = _currentMapStore.Get().GetOrCreateField(id);
        if (field.MovementDifficulty == movementDifficulty)
            return;

        field.MovementDifficulty = movementDifficulty;

        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.FieldUpdated(field).GetAwaiter().GetResult();
    }

    public void AddPlanet(string id, int movementDifficulty, List<ResourceType> resourceTypes)
    {
        _logger.LogDebug("Adding Planet on Field {Id}", id);

        var field = _currentMapStore.Get().GetOrCreateField(id);
        if (field.MovementDifficulty == movementDifficulty &&
            field.Planet != null &&
            field.Planet.ResourceDeposits.Select(d => d.ResourceType).SequenceEqual(resourceTypes))
            return;

        field.MovementDifficulty = movementDifficulty;
        if (field.Planet == null)
        {
            var planet = new Planet
            {
                ResourceDeposits = resourceTypes.Select(type => new ResourceDeposit(type)).ToArray()
            };
            field.SetPlanet(planet);

            _logger.LogDebug("Planet {Planet} added", planet);
        }

        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.FieldUpdated(field).GetAwaiter().GetResult();
    }

    public void AddNeighbour(string fieldId, string neighbourId, int movementDifficulty)
    {
        var mapStore = _currentMapStore.Get();
        var field = mapStore.GetField(fieldId);
        if (field == null)
            throw new Exception($"Field ${fieldId} could not be found");
        var neighbour = mapStore.GetOrCreateField(neighbourId);
        neighbour.MovementDifficulty = movementDifficulty;
        if (!field.IsNeighbour(neighbour))
            field.Map.AddConnection(field, new Connection(neighbour));

        _logger.LogDebug("Added Neighbour {Neighbour} for Field {Field}", neighbourId, fieldId);

        // TODO: Use an async/await pattern somehow
        // TODO: Use a DTO
        _mapHubContext.Clients.All.FieldUpdated(field).GetAwaiter().GetResult();
    }

    public Field GetField(string id)
    {
        return _currentMapStore.Get().GetOrCreateField(id);
    }
}