using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Zero53.Singletons
{
    public abstract class AddressableSOSingleton<T> : ScriptableObject
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
                            // LoadInstance();
                            LoadInstanceBlocked();
                        }
                        return _instance;
                    }
                }
                
                return _instance;
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
            
            var handle = Addressables.LoadAssetAsync<T>(typeof(T).FullName);

            _instance = handle.WaitForCompletion();;
        }
        
        private static void LoadInstance()
        {
#if UNITY_EDITOR
            if (EditorLoadInstance()) return;
#endif
            
            if (_instance != null)
            {
                Addressables.ResourceManager.CreateCompletedOperation(_instance, null);
                return;
            }
            
            var handle = Addressables.LoadAssetAsync<T>(typeof(T).FullName);
            
            handle.Completed += HandleOnCompleted;
        }
        private static bool EditorLoadInstance()
        {
            if (Application.isPlaying) return false;
            
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).FullName}");
            var list = new List<T>();

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var so = AssetDatabase.LoadAssetAtPath<T>(path);
                list.Add(so);
            }

            if (list.Count == 0)
            {
                _instance = CreateInstance<T>();
                AssetDatabase.CreateAsset(_instance, $"Assets/{typeof(T).Name}.asset");
            }
            else
            {
                _instance = list[0];

                for (var i = 1; i < list.Count; i++)
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(guids[i]));
                }
            }
                
            return true;
        }

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

        private void Awake()
        {
            if (_instance != null)
            {
                DestroyImmediate(this);
                Debug.LogError($"Singleton of {typeof(T).FullName} already exists.");
                return;
            }
            
            lock (_lock)
            {
                _instance = this as T;
            }
        }
    }
}