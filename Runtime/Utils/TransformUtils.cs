using System.Collections.Generic;
using UnityEngine;

namespace Zero53.Utils
{
    public static class TransformUtils
    {
        /// <summary>
        /// 获取所有直接子对象
        /// </summary>
        public static IEnumerable<Transform> GetChildren(this Transform transform)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                yield return transform.GetChild(i);
            }
        }
    }
}