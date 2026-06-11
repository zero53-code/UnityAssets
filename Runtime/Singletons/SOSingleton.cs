using UnityEditor;
using UnityEngine;
using Zero53.Utils;

namespace Zero53.Singletons
{
#if UNITY_EDITOR
    
    /// <summary>
    /// [Editor-Only] ScriptableObject 单例类
    /// </summary>
    /// <typeparam name="T">单例类型</typeparam>
    public abstract class SOSingleton<T> : ScriptableObject
        where T : ScriptableObject
    {
        private static volatile T _instance;
        private static readonly object _lock = new();

        public static T instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<T>();

                // 加载项目中所有该类型的 SO
                var assets = ScriptableObjectUtils.FindAllOnEditor<T>();


                if (assets == null || assets.Count == 0)
                {
                    _instance = CreateInstance<T>();
                    _instance.name = typeof(T).Name;
                    
                    AssetDatabase.CreateAsset(_instance, $"Assets/{_instance.name}.asset");
                    return _instance;
                }

                _instance = assets[0];

                return _instance;
            }
        }
      
        public static bool isInitialized => _instance != null;
        
        protected SOSingleton()
        {
            if (_instance != null)
            {
                Debug.LogError($"Singleton of {typeof(T).FullName} already exists.");
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T; 
                return;
            }

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));

            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }
    }
    
#endif
}