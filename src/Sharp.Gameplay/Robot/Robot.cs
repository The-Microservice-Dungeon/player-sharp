using Sharp.Core.Core;
using Sharp.Gameplay.Map;

namespace Sharp.Gameplay.Robot;

public class Robot : IIdentifiable<string>, IFieldLocatable
{
    private readonly string _id;
    private Field _field;
    public bool Alive { get; private set; }

    public Robot(string id, bool alive, RobotAttributes attributes, Field field)
    {
        _id = id;
        Alive = alive;
        _field = field;
        Attributes = attributes;
    }
    
    public string Id => _id;
    public Field Field => _field;

    public RobotAttributes Attributes { get; private set; }

    public Dictionary<ResourceType, uint> Inventory { get; } = new();

    public void Kill() => Alive = false;

    public void SetField(Field field)
    {
        if (!Alive)
            throw new DeadRobotActionException();
        if (!field.IsNeighbour(field))
            throw new IllegalRobotMovementException(Id, field.Id);
        _field = field;
    }
}