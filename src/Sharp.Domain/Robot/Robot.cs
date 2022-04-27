using Sharp.Domain.Core;
using Sharp.Domain.Map;

namespace Sharp.Domain.Robot;

/// <summary>
/// Robot
/// </summary>
public class Robot : IIdentifiable<string>, IFieldLocatable
{
    public Robot(string id, bool alive, RobotAttributes attributes, Field field)
    {
        Id = id;
        Alive = alive;
        Field = field;
        Attributes = attributes;
    }

    public bool Alive { get; private set; }

    public RobotAttributes Attributes { get; }

    public Dictionary<ResourceType, uint> Inventory { get; } = new();
    public Field Field { get; private set; }

    public string Id { get; }

    /// <summary>
    /// Updates robots energy
    /// </summary>
    /// <param name="energy">Energy Amount</param>
    /// <exception cref="DeadRobotActionException">If robot is dead</exception>
    public void UpdateEnergy(uint energy)
    {
        if (!Alive)
            throw new DeadRobotActionException();
        Attributes.Energy = energy;
    }

    /// <summary>
    /// Kills the robot
    /// </summary>
    public void Kill()
    {
        Alive = false;
    }

    /// <summary>
    /// Moves the robot to the specified field
    /// </summary>
    /// <param name="field">Field to move to</param>
    /// <exception cref="DeadRobotActionException">If the robot is dead</exception>
    /// <exception cref="IllegalRobotMovementException">If the field is not reachable for the robot</exception>
    public void Move(Field field)
    {
        if (!Alive)
            throw new DeadRobotActionException();
        if (!Field.IsNeighbour(field))
            throw new IllegalRobotMovementException(Id, field.Id);
        Field = field;
    }
}