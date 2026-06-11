#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zero53.Utils
{
    public static class ScriptableObjectUtils
    {
        /// <summary>
        /// (Editor-Only) 编辑器下查找一个 SO（任意路径）
        /// </summary>
        public static T FindOnEditor<T>() where T : ScriptableObject
        {
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");

            return guids
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<T>)
                .FirstOrDefault(so => so != null);
        }
        
        /// <summary>
        /// (Editor-Only) 编辑器下查找所有SO（任意路径）
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
    }
}

#endif