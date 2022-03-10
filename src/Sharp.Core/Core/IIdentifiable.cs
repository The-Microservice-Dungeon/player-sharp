namespace Sharp.Core;

public interface IIdentifiable<out T>
{
    public T Id { get; }
}