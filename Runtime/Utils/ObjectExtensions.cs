using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Zero53.Utils
{
    public static class ObjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrDestroyed<T>(this T obj)
            where T : class
        {
            if (obj is not UnityEngine.Object uobj) return true;
            return uobj == null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NotDestroyedObject<T>(this T obj)
            where T : class
        {
            if (obj is null) return null;
            if (obj is not UnityEngine.Object uobj) return obj;
            if (uobj == null) return null;
            return uobj as T;
        }

#if UNITY_EDITOR
        
        private static readonly Dictionary<Type, MethodInfo> _typeToOnDrawGizmosMethod = new();
        private static readonly Dictionary<Type, MethodInfo> _typeToOnDrawGizmosSelectedMethod = new();
        private static readonly Dictionary<Type, MethodInfo> _typeToResetMethod = new();
        private static readonly Dictionary<Type, MethodInfo> _typeToOnValidateMethod = new();

#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("UNITY_EDITOR")]
        public static void InvokeOnDrawGizmos(this object obj)
        {
            InvokeMethod(_typeToOnDrawGizmosMethod, "OnDrawGizmos", obj);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("UNITY_EDITOR")]
        public static void InvokeOnDrawGizmosSelected(this object obj)
        {
            InvokeMethod(_typeToOnDrawGizmosSelectedMethod, "OnDrawGizmosSelected", obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("UNITY_EDITOR")]
        public static void InvokeReset(this object obj)
        {
            InvokeMethod(_typeToResetMethod, "Reset", obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("UNITY_EDITOR")]
        public static void InvokeOnValidate(this object obj)
        {
            InvokeMethod(_typeToOnValidateMethod, "OnValidate", obj);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("UNITY_EDITOR")]
        public static void InvokeMethod(Dictionary<Type, MethodInfo> typeToMethodInfo, string methodName, object obj)
        {
            if (obj == null) return;
            if (obj is UnityEngine.Object unityObject && unityObject == null) return;

            var type = obj.GetType();
            if (typeToMethodInfo.TryGetValue(type, out var method))
            {
                method.Invoke(obj, Array.Empty<object>());
            }

            method = type
                .GetMethod(
                    methodName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null) return;

            typeToMethodInfo[type] = method;

            method.Invoke(obj, Array.Empty<object>());
        }
    }
}