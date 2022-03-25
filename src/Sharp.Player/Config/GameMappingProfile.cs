using AutoMapper;
using Sharp.Client.Model;
using Sharp.Core;
using Sharp.Data.Model;
using Sharp.Gameplay.Game;
using Sharp.Gameplay.Trading;
using Sharp.Player.Controllers;

namespace Sharp.Player.Config;

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

    }
}