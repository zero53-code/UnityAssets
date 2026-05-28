using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Zero53.Utils.Attributes;

namespace Zero53.Utils
{
    public class ScriptableObjectLoader<T, TId> where T : ScriptableObject
    {
        private static string[] _addressableKeys;

        public static string[] addressableKeys
        {
            get
            {
                _addressableKeys ??= typeof(T)
                    .GetCustomAttributes(true)
                    .OfType<AddressableKeyAttribute>()
                    .Select(attr => attr.keys)
                    .ToArray()
                    .FirstOrDefault();

                return _addressableKeys;
            }
        }

        private static string _idGetterName;

        public static string idGetterName
        {
            get
            {
                _idGetterName ??= typeof(T)
                    .GetCustomAttributes(true)
                    .OfType<IdGetterAttribute>()
                    .Select(attr => attr.name)
                    .FirstOrDefault();

                return _idGetterName;
            }
        }
        
        public static Func<T, TId> idGetter
        {
            get
            {
                return obj =>
                {
                    GetGetter(idGetterName);
                    
                    if (_idField != null)
                    {
                        return (TId)_idField.GetValue(obj);
                    }

                    if (_idProp != null)
                    {
                        return (TId)_idProp.GetValue(obj);
                    }

                    if (_idGetterMethod != null)
                    {
                        return (TId)_idGetterMethod.Invoke(obj, null);
                    }
                    
                    throw new Exception($"No named '{idGetterName}' of getter found for '{typeof(T).FullName}'.");
                };
            }
        }
        
        private readonly List<T> _loadedAssets = new();
        private readonly Dictionary<TId, T> _loadedAssetsById = new();

        public ScriptableObjectLoader()
        {
#if UNITY_EDITOR

            if (!Application.isPlaying)
            {
                UnityEngine.Debug.Log($"Loading ScriptableObject at key {addressableKeys}");
                var handleOnEditor = Addressables.LoadResourceLocationsAsync(addressableKeys, typeof(T));
                var list = handleOnEditor.WaitForCompletion();
                
                _loadedAssets.AddRange(list.Select(loc => loc.Data as T));
                foreach (var asset in _loadedAssets)
                {
                    UnityEngine.Debug.Log($"Loaded asset name: {asset.name}");
                    _loadedAssetsById[idGetter(asset)] = asset;
                }
                
                return;
            }
            
#endif
            
            var handle = Addressables.LoadResourceLocationsAsync(addressableKeys, typeof(T));

            handle.Completed += OnHandleOnCompleted;
        }

        private void OnHandleOnCompleted(AsyncOperationHandle<IList<IResourceLocation>> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _loadedAssets.AddRange(handle.Result.Select(res => res.Data as T));
                foreach (var asset in _loadedAssets)
                {
                    _loadedAssetsById[idGetter(asset)] = asset;
                }
            }

            handle.Release();
        }

        [CanBeNull]
        public T LoadAsset(TId id)
        {
            return _loadedAssetsById[id];
        }

        private static FieldInfo _idField;
        private static PropertyInfo _idProp;
        private static MethodInfo _idGetterMethod;
        
        private static bool GetGetter(string getterName)
        {
            if (string.IsNullOrEmpty(getterName)) return false;
            if (_idField != null || _idProp != null || _idGetterMethod != null) return true;

            var type = typeof(T);
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            // 字段
            _idField = type.GetField(getterName, flags);
            if (_idField != null) return true;

            // 属性
            _idProp = type.GetProperty(getterName, flags);
            if (_idProp != null) return true;

            // 方法
            _idGetterMethod = type.GetMethod(getterName, flags);
            if (_idGetterMethod != null && _idGetterMethod.GetParameters().Length == 0)
                return true;

            return false;
        }
    }
}