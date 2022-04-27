namespace Sharp.Domain.Core;

public interface ISingleStateStore<T>
    where T : class
{
    T Get();
    void Set(T data);
    bool IsSet();
}