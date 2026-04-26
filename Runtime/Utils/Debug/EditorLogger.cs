using System;
using System.Diagnostics;
using Object = UnityEngine.Object;

namespace Zero53.Utils.Debug
{
    public static class EditorLogger
    {
        [Conditional("UNITY_EDITOR")]
        public static void Info(string message, Object context = null)
        {
            if (context == null) UnityEngine.Debug.Log(message);
            else UnityEngine.Debug.Log(message, context);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Warning(string message, Object context = null)
        {
            if (context == null) UnityEngine.Debug.LogWarning(message);
            else UnityEngine.Debug.LogWarning(message, context);
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void Error(string message, Object context = null)
        {
            if (context == null) UnityEngine.Debug.LogError(message);
            else UnityEngine.Debug.LogError(message, context);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Exception(Exception exception, Object context = null)
        {
            if (context == null) UnityEngine.Debug.LogException(exception);
            else UnityEngine.Debug.LogException(exception, context);
        }
    }
}