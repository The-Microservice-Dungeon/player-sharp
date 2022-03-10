using Player.Sharp.Core;
using Player.Sharp.Data;

namespace Player.Sharp.Services;

public class MapService
{
    private readonly ILogger<MapService> _logger;
    private readonly IMapRepository _mapRepository;

    public MapService(IMapRepository mapRepository, ILogger<MapService> logger)
    {
        _mapRepository = mapRepository;
        _logger = logger;
    }

    public Map CreateMap(string mapId, IEnumerable<string> spaceStationIds)
    {
        var map = new Map(mapId);
        var spaceStations = spaceStationIds.Select(s => new Spacestation(s));
        map.spacestations.UnionWith(spaceStations);
        _mapRepository.Save(map);
        return map;
    }

    public Spacestation GetRandomSpacestationFromActiveMap()
    {
        // Random enough.
        return _mapRepository.GetActiveMap().spacestations.First();
    }
}