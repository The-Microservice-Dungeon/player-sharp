using AutoMapper;
using Sharp.Gameplay.Map;
using Sharp.Player.Controllers;

namespace Sharp.Player.Config;

// TODO: Refactor
public class MapMappingProfile : Profile
{
    public MapMappingProfile()
    {
        CreateMap<List<Connection>, string[]>()
            .ConstructUsing(connections => connections.Select(c => c.Destination.Id).ToArray());

        CreateMap<Planet, PlanetDto>();
        CreateMap<SpaceStation, SpacestationDto>();
        CreateMap<ResourceDeposit, ResourceDepositDto>();

        CreateMap<KeyValuePair<Field, List<Connection>>, FieldDto>()
            .ConstructUsing((kv, ctx) => new FieldDto(ctx.Mapper.Map<string[]>(kv.Value))
            {
                Planet = ctx.Mapper.Map<PlanetDto>(kv.Key.Planet),
                Spacestation = ctx.Mapper.Map<SpacestationDto>(kv.Key.SpaceStation),
                MovementDifficulty = kv.Key.MovementDifficulty
            });

        CreateMap<KeyValuePair<Field, List<Connection>>, KeyValuePair<string, FieldDto>>()
            .ConstructUsing((kv, ctx) => new KeyValuePair<string, FieldDto>(kv.Key.Id, ctx.Mapper.Map<FieldDto>(kv)));

        CreateMap<Map, MapDto>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("fields", opt => opt.MapFrom(src => src.Fields));
    }
}