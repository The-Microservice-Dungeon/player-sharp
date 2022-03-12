namespace Sharp.Gameplay.Map;

public struct Connection
{
    public Field Source;
    public Field Destination;
    
    public Connection(Field source, Field destination)
    {
        Source = source;
        Destination = destination;
    }
}