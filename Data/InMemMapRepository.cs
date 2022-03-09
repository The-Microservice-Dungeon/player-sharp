using Player.Sharp.Core;

namespace Player.Sharp.Data
{
    public class InMemMapRepository : IMapRepository
    {
        private Map? _activeMap;
        public Map GetActiveMap()
        {
            if(_activeMap == null)
            {
                throw new ApplicationException("Map is null. Shouldn't be so.");
            }
            return _activeMap;
        }

        public void Save(Map map)
        {
            _activeMap = map;
        }
    }
}
