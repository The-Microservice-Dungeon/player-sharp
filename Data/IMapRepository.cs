using Player.Sharp.Core;

namespace Player.Sharp.Data
{
    public interface IMapRepository
    {
        Map GetActiveMap();
        void Save(Map map);
    }
}
