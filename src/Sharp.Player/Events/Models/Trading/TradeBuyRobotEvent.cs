using System.Text.Json.Serialization;

namespace Sharp.Player.Events.Models.Trading;

public class TradeBuyRobotEvent : TradeBaseEvent<TradeRobotData[]>
{
    
}

public class TradeRobotData
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("player")]
    public string Player { get; set; }
    
    [JsonPropertyName("planet")]
    public string Planet { get; set; }
    
    [JsonPropertyName("alive")]
    public bool Alive { get; set; }
    
    [JsonPropertyName("maxHealth")]
    public uint MaxHealth { get; set; }
    
    [JsonPropertyName("maxEnergy")]
    public uint MaxEnergy { get; set; }
    
    [JsonPropertyName("energyRegen")]
    public uint EnergyRegen { get; set; }
    
    [JsonPropertyName("attackDamage")]
    public uint AttackDamage { get; set; }
    
    [JsonPropertyName("miningSpeed")]
    public uint MiningSpeed { get; set; }
    
    [JsonPropertyName("health")]
    public uint Health { get; set; }
    
    [JsonPropertyName("energy")]
    public uint Energy { get; set; }
    
    [JsonPropertyName("healthLevel")]
    public uint HealthLevel { get; set; }
    
    [JsonPropertyName("damageLevel")]
    public uint DamageLevel { get; set; }
    
    [JsonPropertyName("miningSpeedLevel")]
    public uint MiningSpeedLevel { get; set; }
    
    [JsonPropertyName("miningLevel")]
    public uint MiningLevel { get; set; }
    
    [JsonPropertyName("energyLevel")]
    public uint EnergyLevel { get; set; }
    
    [JsonPropertyName("energyRegenLevel")]
    public uint EnergyRegenLevel { get; set; }
    
    [JsonPropertyName("storageLevel")]
    public uint StorageLevel { get; set; }
    
    [JsonPropertyName("inventory")]
    public TradeRobotInventoryData Inventory { get; set; }
    
    [JsonPropertyName("items")]
    public TradeRobotItemsData Items { get; set; }
    
}

public class TradeRobotInventoryData
{
    [JsonPropertyName("maxStorage")] 
    public uint MaxStorage { get; set; }
    
    [JsonPropertyName("usedStorage")]
    public uint UsedStorage { get; set; }
    
    [JsonPropertyName("storedCoal")]
    public uint StoredCoal { get; set; }
    
    [JsonPropertyName("storedIron")]
    public uint StoredIron { get; set; }
    
    [JsonPropertyName("storedGem")]
    public uint StoredGem { get; set; }
    
    [JsonPropertyName("storedGold")]
    public uint StoredGold { get; set; }
    
    [JsonPropertyName("storedPlatin")]
    public uint StoredPlatin { get; set; }
}

public class TradeRobotItemsData
{
    [JsonPropertyName("rocket")]
    public uint Rocket { get; set; }
    
    [JsonPropertyName("wormhole")]
    public uint Wormhole { get; set; }
    
    [JsonPropertyName("longRangeBombardement")]
    public uint LongRangeBombardement { get; set; }
    
    [JsonPropertyName("selfDestruction")]
    public uint SelfDestruction { get; set; }
    
    [JsonPropertyName("repairSwarm")]
    public uint RepairSwarm { get; set; }
    
    [JsonPropertyName("nuke")]
    public uint Nuke { get; set; }
}