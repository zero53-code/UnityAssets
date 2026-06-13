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
                    if (IsUnitySerializable(field))
                    {
                        fields.Add(field);
                    }
                }
                type = type.BaseType; // 遍历父类
            }
            return fields;
        }

        /// <summary>
        /// 判断一个类型是否会被 Unity 序列化
        /// </summary>
        public static bool IsUnitySerializable(Type type)
        {
            if (typeof(UnityEngine.Object).IsAssignableFrom(type)) return true;
    
            if (type.IsPrimitive) return true;
            if (type.IsEnum) return true;
            if (type == typeof(string)) return true;
            if (type == typeof(decimal)) return true;
    
            if (type.IsArray) 
            {
                return type.GetArrayRank() == 1 && IsUnitySerializable(type.GetElementType());
            }
    
            if (type.IsGenericType) 
            {
                // 所有泛型参数都必须可序列化
                foreach (var genericArgument in type.GetGenericArguments())
                {
                    if (!IsUnitySerializable(genericArgument)) return false;
                }
                
                return false;
            }
    
            // 检查是否为 UnityEngine 内置结构体
            if (type == typeof(Vector2) ||
                type == typeof(Vector3) ||
                type == typeof(Vector4) ||
                type == typeof(Quaternion) ||
                type == typeof(Matrix4x4) ||
                type == typeof(Color) ||
                type == typeof(Color32) ||
                type == typeof(Rect) ||
                type == typeof(Bounds) ||
                type == typeof(AnimationCurve) ||
                type == typeof(Gradient) ||
                type == typeof(LayerMask)) return true;
    
            if (type.IsValueType && !type.IsPrimitive && !type.IsEnum)
            {
                // 对于自定义结构体，检查是否被 [Serializable] 标记
                if (type.IsDefined(typeof(SerializableAttribute), true)) return true;
            }

            // 其他情况默认不可序列化
            return false;
        }


        /// <summary>
        /// 判断一个字段是否会被 Unity 序列化
        /// </summary>
        public static bool IsUnitySerializable(FieldInfo field)
        {
            if (field.IsStatic) return false;
            if (field.IsInitOnly) return false;

            var isPublic = field.IsPublic;
            var hasSerializeField = Attribute.IsDefined(field, typeof(SerializeField));

            if (!isPublic && !hasSerializeField) return false;

            var fieldType = field.FieldType;
            return IsUnitySerializable(fieldType);
        }
    }
}