namespace Sharp.Gameplay.Robot;

public readonly record struct RobotAttributes
(
    uint MaxHealth,
    uint MaxEnergy,
    uint EnergyRegen,
    uint AttackDamage,
    uint MiningSpeed,
    uint Health,
    uint Energy,
    uint HealthLevel,
    uint DamageLevel,
    uint MiningSpeedLevel,
    uint MiningLevel,
    uint EnergyLevel,
    uint EnergyRegenLevel,
    uint StorageLevel
)
{
}