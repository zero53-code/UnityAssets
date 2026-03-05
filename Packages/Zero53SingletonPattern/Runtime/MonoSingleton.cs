using UnityEngine;

namespace Zero53.SingletonPattern
{
    public class MonoSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
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
                                var go = new GameObject
                                {
                                    name = typeof(T).Name
                                };
                                _instance = go.AddComponent<T>();
                                DontDestroyOnLoad(go);
                            }
                        }
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"{nameof(MonoSingleton<T>)} already exists.");
                Destroy(gameObject);
            }
        }
    }
}