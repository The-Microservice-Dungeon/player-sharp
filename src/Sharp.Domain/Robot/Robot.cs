﻿using Sharp.Domain.Core;
using Sharp.Domain.Map;

namespace Sharp.Domain.Robot;

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

    public void UpdateEnergy(uint energy)
    {
        Attributes.Energy = energy;
    }

    public void Kill()
    {
        Alive = false;
    }

    public void SetField(Field field)
    {
        if (!Alive)
            throw new DeadRobotActionException();
        if (!Field.IsNeighbour(field))
            throw new IllegalRobotMovementException(Id, field.Id);
        Field = field;
    }
}