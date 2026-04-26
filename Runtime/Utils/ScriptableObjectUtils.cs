using System.Collections.Generic;
using UnityEngine;

namespace Zero53.Utils
{
    public static class ScriptableObjectUtils
    {
        /// <summary>
        /// 运行时加载所有指定类型SO（必须在Resources下）
        /// </summary>
        public static List<T> LoadAll<T>(string path = "") where T : ScriptableObject
        {
            var result = new List<T>();
            var assets = Resources.LoadAll<T>(path);

            if (assets != null)
                result.AddRange(assets);
        
            return result;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 编辑器下查找所有SO（任意路径）
        /// </summary>
        public static List<T> FindAllOnEditor<T>() where T : ScriptableObject
        {
            var result = new List<T>();
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var so = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                if (so != null) result.Add(so);
            }
        
            return result;
        }
#endif
    }
}