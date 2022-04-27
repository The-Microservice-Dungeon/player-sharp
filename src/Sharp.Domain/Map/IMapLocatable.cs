namespace Sharp.Domain.Map;

/// <summary>
///     Represents something that can be placed inside a map. This can be a field
/// </summary>
public interface IMapLocatable
{
    public Map Map { get; }
}