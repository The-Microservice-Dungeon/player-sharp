using Player.Sharp.Core;

namespace Player.Sharp.Data
{
    public interface IRobotRepository
    {
        IEnumerable<Robot> FindAll();
        Robot FindById(string id);
        uint Count();
        void Save(Robot robot);
    }
}
