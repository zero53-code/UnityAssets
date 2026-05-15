namespace Zero53.Singletons
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static volatile T _instance;
        private static readonly object _lock = new();

        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }
        public static bool isInitialized => _instance != null;
    }
}