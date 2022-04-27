namespace Sharp.Domain.Core;

/// <summary>
///     Simple interface to compose an identifiable type
/// </summary>
/// <typeparam name="T">type of the identifier</typeparam>
public interface IIdentifiable<out T>
{
    public T Id { get; }
}