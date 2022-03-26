namespace Sharp.Gameplay.Robot;

public class IllegalRobotMovementException : Exception
{
    public IllegalRobotMovementException()
    {
    }

    public IllegalRobotMovementException(string robotId, string fieldId)
        : base($"Illegal movement for robot {robotId} to field {fieldId}")
    {
    }
}