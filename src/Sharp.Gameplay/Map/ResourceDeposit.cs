namespace Sharp.Gameplay.Map;

/// <summary>
///     A resource deposit which contains a specific resource
/// </summary>
public class ResourceDeposit
{
    public readonly ResourceType ResourceType;

    public ResourceDeposit(ResourceType resourceType)
    {
        ResourceType = resourceType;
    }
}