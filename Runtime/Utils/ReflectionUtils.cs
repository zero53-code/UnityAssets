using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Zero53.Utils
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// 获取一个对象身上所有被 Unity 序列化的字段
        /// </summary>
        public static List<FieldInfo> GetSerializedFields(object target)
        {
            var fields = new List<FieldInfo>();
            if (target == null) return fields;

            var type = target.GetType();
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

            while (type != null && type != typeof(object))
            {
                foreach (var field in type.GetFields(flags))
                {
                    if (IsUnitySerializedField(field))
                    {
                        fields.Add(field);
                    }
                }
                type = type.BaseType; // 遍历父类
            }
            return fields;
        }

        /// <summary>
        /// 判断一个字段是否会被 Unity 序列化
        /// </summary>
        public static bool IsUnitySerializedField(FieldInfo field)
        {
            // 排除静态、常量、序列化忽略
            if (field.IsStatic || field.IsLiteral || field.IsNotSerialized)
                return false;

            // 排除 [NonSerialized]
            if (field.GetCustomAttribute<NonSerializedAttribute>() != null)
                return false;

            // public → 序列化
            if (field.IsPublic)
                return true;

            // private + [SerializeField] → 序列化
            if (field.GetCustomAttribute<SerializeField>() != null)
                return true;

            return false;
        }
    }
}