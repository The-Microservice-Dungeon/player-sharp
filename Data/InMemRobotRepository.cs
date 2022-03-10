using Player.Sharp.Core;

namespace Player.Sharp.Data;

public class InMemRobotRepository : IRobotRepository
{
    private readonly Dictionary<string, Robot> _storage = new();

    public uint Count()
    {
        return (uint)_storage.Count;
    }

    public IEnumerable<Robot> FindAll()
    {
        return _storage.Values;
    }

    public Robot FindById(string id)
    {
        return _storage[id];
    }

    public void Save(Robot robot)
    {
        _storage.Add(robot.ID, robot);
    }

    public void RemoveById(string id)
    {
        if (_storage.ContainsKey(id))
            _storage.Remove(id);
    }
}