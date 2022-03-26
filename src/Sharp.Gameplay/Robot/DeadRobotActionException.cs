﻿namespace Sharp.Gameplay.Robot;

public class DeadRobotActionException : Exception
{
    public DeadRobotActionException()
    {
    }

    public DeadRobotActionException(string robotId)
        : base($"Robot with Id {robotId} is killed")
    {
    }
}