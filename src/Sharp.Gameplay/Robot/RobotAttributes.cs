namespace Sharp.Gameplay.Robot;

public class RobotAttributes
{
    public uint MaxHealth { get; }
    public  uint MaxEnergy { get; }
    public  uint EnergyRegen { get; }
    public  uint AttackDamage { get; }
    public  uint MiningSpeed { get; }
    public  uint Health { get; set; }
    public  uint Energy { get; set; }
    public  uint HealthLevel { get; }
    public  uint DamageLevel { get; }
    public  uint MiningSpeedLevel { get; }
    public  uint MiningLevel { get; }
    public  uint EnergyLevel { get; }
    public  uint EnergyRegenLevel { get; }
    public  uint StorageLevel { get; }
    public  uint MaxStorage { get; }

    public RobotAttributes(uint maxHealth, uint maxEnergy, uint energyRegen, uint attackDamage, uint miningSpeed, uint health, uint energy, uint healthLevel, uint damageLevel, uint miningSpeedLevel, uint miningLevel, uint energyLevel, uint energyRegenLevel, uint storageLevel, uint maxStorage)
    {
        MaxHealth = maxHealth;
        MaxEnergy = maxEnergy;
        EnergyRegen = energyRegen;
        AttackDamage = attackDamage;
        MiningSpeed = miningSpeed;
        Health = health;
        Energy = energy;
        HealthLevel = healthLevel;
        DamageLevel = damageLevel;
        MiningSpeedLevel = miningSpeedLevel;
        MiningLevel = miningLevel;
        EnergyLevel = energyLevel;
        EnergyRegenLevel = energyRegenLevel;
        StorageLevel = storageLevel;
        MaxStorage = maxStorage;
    }
}