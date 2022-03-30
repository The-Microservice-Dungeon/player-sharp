namespace Sharp.Player.Repository;

public class UnsetStateException : Exception
{
    public UnsetStateException()
    {
    }

    public UnsetStateException(string message) : base(message)
    {
    }
}