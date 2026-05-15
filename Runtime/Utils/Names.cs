using System.Collections.Generic;

namespace Zero53.Utils
{
    public static class Names
    {
        private static readonly Dictionary<string, int> _nameId = new();
        private static readonly Dictionary<int, string> _idToName = new();

        private static int _nextId;
        
        public static int GetId(this string name)
        {
            if (_nameId.TryGetValue(name, out var result))
            {
                return result;
            }
            
            _nameId[name] = _nextId;
            _idToName[_nextId] = name;
            
            result = _nextId;
            _nextId++;
            
            return result;
        }

        public static string GetName(int id)
        {
            return _idToName.GetValueOrDefault(id);
        }
    }
}