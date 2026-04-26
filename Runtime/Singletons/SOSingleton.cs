using UnityEngine;

namespace Zero53.Singletons
{
    public class SOSingleton<T> : ScriptableObject
        where T : ScriptableObject
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
                            _instance = FindObjectOfType<T>();

                            if (_instance == null)
                            {
                                // 如果已经存在，直接返回
                                if (_instance != null)
                                    return _instance;

                                // 加载项目中所有该类型的SO（自动找到唯一实例）
                                var assets = Resources.FindObjectsOfTypeAll<T>();

                                if (assets == null || assets.Length == 0)
                                {
                                    Debug.LogError($"Singleton of {typeof(T).FullName} could not be found.");
                                    return null;
                                }

                                if (assets.Length > 1)
                                {
                                    Debug.LogError($"{typeof(T).FullName} already exists.");
                                    return null;
                                }

                                _instance = assets[0];
                                return _instance;
                            }
                        }
                    }
                }

                return _instance;
            }
        }
        
        protected SOSingleton()
        {
            if (_instance != null)
            {
                Debug.LogError($"Singleton of {typeof(T).FullName} already exists.");
            }
        }
    }
}