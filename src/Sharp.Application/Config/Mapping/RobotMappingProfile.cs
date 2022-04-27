using AutoMapper;
using Sharp.Domain.Robot;
using Sharp.Player.Events.Models.Trading;

namespace Sharp.Player.Config.Mapping;

public class RobotMappingProfile : Profile
{
    public RobotMappingProfile()
    {
        CreateMap<TradeRobotData, RobotAttributes>()
            // Urgh
            .ConstructUsing(from => new RobotAttributes(from.MaxHealth, from.MaxEnergy, from.EnergyRegen,
                from.AttackDamage, from.MiningSpeed, from.Health, from.Energy, from.HealthLevel, from.DamageLevel,
                from.MiningSpeedLevel, from.MiningLevel, from.EnergyLevel, from.EnergyRegenLevel, from.StorageLevel,
                from.Inventory.MaxStorage));
    }
}