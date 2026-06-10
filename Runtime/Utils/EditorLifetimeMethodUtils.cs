using System;
using System.Collections.Generic;
using System.Reflection;

#if UNITY_EDITOR

namespace Zero53.Utils
{
    public static class EditorLifetimeMethodUtils
    {
        private static readonly Dictionary<Type, MethodInfo> _typeToOnDrawGizmosMethod = new();
        private static readonly Dictionary<Type, MethodInfo> _typeToOnDrawGizmosSelectedMethod = new();
        private static readonly Dictionary<Type, MethodInfo> _typeToResetMethod = new();
        private static readonly Dictionary<Type, MethodInfo> _typeToOnValidateMethod = new();

        public static void InvokeOnDrawGizmos(this object obj)
        {
            InvokeMethod(_typeToOnDrawGizmosMethod, "OnDrawGizmos", obj);
        }
        
        public static void InvokeOnDrawGizmosSelected(this object obj)
        {
            InvokeMethod(_typeToOnDrawGizmosSelectedMethod, "OnDrawGizmosSelected", obj);
        }

        public static void InvokeReset(this object obj)
        {
            InvokeMethod(_typeToResetMethod, "Reset", obj);
        }

        public static void InvokeOnValidate(this object obj)
        {
            InvokeMethod(_typeToOnValidateMethod, "OnValidate", obj);
        }
        
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

#endif