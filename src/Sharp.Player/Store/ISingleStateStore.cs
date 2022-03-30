namespace Sharp.Player.Repository;

public interface ISingleStateStore<T>
    where T : class
{
    T Get();
    void Set(T data);
    bool IsSet();
}