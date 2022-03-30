namespace Sharp.Player.Repository;

public class UnsetStateException : Exception
{
    public UnsetStateException() : base() {}
    
    public UnsetStateException(string message) : base(message) {}
}