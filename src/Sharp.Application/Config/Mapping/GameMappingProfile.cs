using AutoMapper;
using Sharp.Domain.Game;
using Sharp.Domain.Trading;
using Sharp.Infrastructure.Network.Model;
using Sharp.Infrastructure.Persistence.Models;
using Sharp.Player.Controllers;

namespace Sharp.Player.Config.Mapping;

public class GameMappingProfile : Profile
{
    public GameMappingProfile()
    {
        CreateMap<GameResponse, Game>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.GameId))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GameId));
        CreateMap<GameRegistration, Game>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.GameId))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GameId));
        CreateMap<Game, GameDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        // Lel
        CreateMap<Item, string>()
            .ConstructUsing(item => item.ToString().ToUpper());
        CreateMap<CommandType, string>()
            .ConstructUsing(type => type.ToString().ToLower());
        CreateMap<BaseCommand, CommandRequest>();
        CreateMap<BaseCommandObject, CommandObjectRequest>();
    }
}