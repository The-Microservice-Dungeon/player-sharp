namespace Sharp.Gameplay.Map;

/// <summary>
/// Represents something that can be placed inside a field. Could be a space-station, planet or other things
/// thinkable in future.
/// Possibly a robot is also locatable. 
/// </summary>
public interface IFieldLocatable
{
    public Field Location { get; }
}