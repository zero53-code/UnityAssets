using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zero53.Singletons.Attributes;
using Zero53.Utils;

namespace Zero53.Singletons
{
    public abstract class AddressableSOSingleton<T> : ScriptableObject
        where T : ScriptableObject
    {
        private static volatile T _instance;
        private static readonly object _lock = new();
        
        private static string _addressableKey;

        public static string addressableKey
        {
            get
            {
                _addressableKey ??= typeof(T)
                    .GetCustomAttributes(true)
                    .OfType<AddressableKeyAttribute>()
                    .Select(attr => attr.key)
                    .FirstOrDefault();

                _addressableKey ??= typeof(T).Name.Replace('.', '/');

                return _addressableKey;
            }
        }

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
                            LoadInstanceBlocked();
                        }
                        return _instance;
                    }
                }
                
                return _instance;
            }
        }

        public static Task<T> instanceAsync
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            return LoadInstance();
                        }
                    }
                }
                
                return new Task<T>(() => _instance);
            }
        }
        
        public static bool isInitialized => _instance != null;

        public static void Release()
        {
#if UNITY_EDITOR
            
            if (!EditorApplication.isPlaying)
            {
                _instance = null;
                return;
            }
            
#endif
            
            lock (_lock)
            {
                Addressables.Release(_instance);
                _instance = null;
            }
        }

        private static void LoadInstanceBlocked()
        {
#if UNITY_EDITOR

            if (EditorLoadInstance()) return;
#endif
            var handle = Addressables.LoadAssetAsync<T>(addressableKey);

            _instance = handle.WaitForCompletion();;
        }
        
        private static Task<T> LoadInstance()
        {
#if UNITY_EDITOR
            if (EditorLoadInstance()) return new Task<T>(() => instance);
#endif
            
            if (_instance != null)
            {
                return Addressables
                    .ResourceManager
                    .CreateCompletedOperation(_instance, null)
                    .Task;
            }
            
            return Addressables
                .LoadAssetAsync<T>(addressableKey)
                .Task;
        }

#if UNITY_EDITOR
        private static bool EditorLoadInstance()
        {
            if (Application.isPlaying) return false;
            
            var list = ScriptableObjectUtils.FindAllOnEditor<T>();
            if (list == null || list.Count == 0)
            {
                _instance = CreateInstance<T>();
                AssetDatabase.CreateAsset(_instance, $"Assets/{typeof(T).Name}.asset");
            }
            else
            {
                _instance = list[0];
                for (var i = 1; i < list.Count; i++)
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(list[i]));
                }
            }

            return true;
        }
        
#endif
        private static void HandleOnCompleted(AsyncOperationHandle<T> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)  
            {  
                _instance = handle.Result;
                Debug.Log($"{typeof(T).FullName} instance completed");
            }  
            else  
            {  
                Debug.LogError($"Failed to load AddressableSOSingleton: {handle.OperationException}");  
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance = this as T;
                }
                return;
            }
            
#if UNITY_EDITOR
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
#endif
            DestroyImmediate(this);
            Debug.LogError($"Singleton of {typeof(T).FullName} already exists.");
        }

        protected virtual void OnEnable()
        {
            if (_instance == null) return;

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
        }
    }
}