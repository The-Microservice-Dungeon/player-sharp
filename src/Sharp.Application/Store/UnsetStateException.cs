namespace Sharp.Player.Store;

public class UnsetStateException : Exception
{
    public UnsetStateException()
    {
    }

    public UnsetStateException(string message) : base(message)
    {
    }
}