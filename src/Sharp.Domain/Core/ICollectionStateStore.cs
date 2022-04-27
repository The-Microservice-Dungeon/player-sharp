namespace Sharp.Domain.Core;

public interface ICollectionStateStore<T> 
    where T: class
{
    IEnumerable<T> Get();
    void Add(T t);
    void Clear();
}