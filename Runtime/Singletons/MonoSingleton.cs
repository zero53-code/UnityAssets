using UnityEngine;

namespace Zero53.Singletons
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
#if UNITY_EDITOR
                                if (!Application.isPlaying) return null;
#endif
                                var go = new GameObject
                                {
                                    name = typeof(T).Name
                                };
                                _instance = go.AddComponent<T>();

#if UNITY_EDITOR
                                if (_instance.transform.parent == null) 
                                    DontDestroyOnLoad(go);
#else
                                transform.parent = null;
                                DontDestroyOnLoad(go);
#endif
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
                
                if (gameObject.transform.parent != null) return;
                
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.LogError($"{typeof(T).FullName} already exists.");
                Destroy(gameObject);
            }
        }
    }
}