namespace Sharp.Gameplay.Map;

public class Connection
{
    public Field Source;
    public Field Destination;
    public int MovementDifficulty = 0;
    
    public Connection(Field source, Field destination)
    {
        Source = source;
        Destination = destination;
    }

    public Connection(Field source, Field destination, int movementDifficulty) : this(source, destination)
    {
        MovementDifficulty = movementDifficulty;
    }
}