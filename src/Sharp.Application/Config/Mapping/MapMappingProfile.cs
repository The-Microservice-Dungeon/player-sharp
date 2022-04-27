using AutoMapper;
using Sharp.Domain.Map;
using Sharp.Player.Controllers;

namespace Sharp.Player.Config.Mapping;

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

        CreateMap<KeyValuePair<Field, List<Connection>>, KeyValuePair<string, FieldDto>>()
            .ConstructUsing((kv, ctx) => new KeyValuePair<string, FieldDto>(kv.Key.Id, ctx.Mapper.Map<FieldDto>(kv)));

        CreateMap<KeyValuePair<Field, List<Connection>>, FieldDto>()
            .ConstructUsing((kv, ctx) => new FieldDto(ctx.Mapper.Map<string[]>(kv.Value)))
            .ForMember(dest => dest.Planet, opt => opt.MapFrom(src => src.Key.Planet))
            .ForMember(dest => dest.Spacestation, opt => opt.MapFrom(src => src.Key.SpaceStation))
            .ForMember(dest => dest.MovementDifficulty, opt => opt.MapFrom(src => src.Key.MovementDifficulty))
            .ForMember(dest => dest.Connections, opt => opt.MapFrom(src => src.Value));

        CreateMap<Map, MapDto>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("fields", opt => opt.MapFrom(src => src.Fields));
    }
}