using AutoMapper;
using Sharp.Gameplay.Robot;
using Sharp.Player.Events.Models.Trading;

namespace Sharp.Player.Config.Mapping;

public class RobotMappingProfile : Profile
{
    public RobotMappingProfile()
    {
        CreateMap<TradeRobotData, RobotAttributes>()
            .ForMember(dest => dest.MaxStorage, opt => opt.MapFrom(src => src.Inventory.MaxStorage));
    }
}