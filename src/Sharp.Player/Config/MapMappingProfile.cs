using System.Collections.Immutable;
using AutoMapper;
using Sharp.Gameplay.Map;
using Sharp.Player.Controllers;

namespace Sharp.Player.Config;

public class MapMappingProfile : Profile
{
    public MapMappingProfile()
    {
        CreateMap<ImmutableList<Connection>, string[]>()
            .ConstructUsing(connections => connections.Select(c => c.Destination.Id).ToArray());

        CreateMap<Planet, PlanetDto>();
        CreateMap<SpaceStation, SpacestationDto>();
        
        CreateMap<Field, FieldDto>()
            .ForCtorParam("connections", o => o.MapFrom(src => src.Connections));

        CreateMap<Field, KeyValuePair<string, FieldDto>>()
            .ConstructUsing((f, ctx) => new KeyValuePair<string, FieldDto>(f.Id, ctx.Mapper.Map<FieldDto>(f)));

        CreateMap<Map, MapDto>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("fields", opt => opt.MapFrom(src => src.Fields));
    }
}