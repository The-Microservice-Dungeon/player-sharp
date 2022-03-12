using AutoMapper;
using Sharp.Client.Model;
using Sharp.Core;

namespace Sharp.Player.Config;

public class GameMappingProfile : Profile
{
    public GameMappingProfile()
    {
        CreateMap<GameResponse, Game>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.GameId))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GameId));
    }
}