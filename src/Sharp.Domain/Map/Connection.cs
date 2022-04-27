namespace Sharp.Domain.Map;

/// <summary>
///     Connection between fields. Assuming they are unidirectional
/// </summary>
public struct Connection
{
    public Field Destination;

    public Connection(Field destination)
    {
        Destination = destination;
    }
}