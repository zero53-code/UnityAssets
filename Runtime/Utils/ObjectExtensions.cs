using System.Runtime.CompilerServices;

namespace Zero53.Utils
{
    public static class ObjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNull<T>(this T obj)
            where T : class
        {
            if (obj is not UnityEngine.Object uobj) return true;
            return uobj == null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Ref<T>(this T obj)
            where T : class
        {
            if (obj is null) return null;
            if (obj is not UnityEngine.Object uobj) return obj;
            if (uobj == null) return null;
            return uobj as T;
        }
    }
}