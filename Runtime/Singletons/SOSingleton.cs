using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Zero53.Singletons
{
    /// <summary>
    /// 基于 Resources 文件夹的 ScriptableObject 单例类
    /// </summary>
    /// <typeparam name="T">单例类型</typeparam>
    public abstract class SOSingleton<T> : ScriptableObject
        where T : ScriptableObject
    {
        private static volatile T _instance;
        private static readonly object _lock = new();

#if !UNITY_EDITOR
        
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
      
#else
        public static T instance
        {
            get
            {
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

                return _instance;
            }
        }
        
#endif
        
        public static bool isInitialized => _instance != null;
        
        protected SOSingleton()
        {
            if (_instance != null)
            {
                Debug.LogError($"Singleton of {typeof(T).FullName} already exists.");
            }
        }
    }
}