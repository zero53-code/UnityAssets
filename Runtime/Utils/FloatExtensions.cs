using System.Runtime.CompilerServices;
using UnityEngine;

namespace Zero53.Utils
{
    public static class FloatExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Abs(this float value) => Mathf.Abs(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(this float a, float b) => Mathf.Max(a, b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(this float a, float b) => Mathf.Min(a, b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(this float value, float min, float max) => Mathf.Clamp(value, min, max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp01(this float value) => Mathf.Clamp01(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sin(this float value) => Mathf.Sin(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cos(this float value) => Mathf.Cos(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Tan(this float value) => Mathf.Tan(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Asin(this float value) => Mathf.Asin(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Acos(this float value) => Mathf.Acos(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atan(this float value) => Mathf.Atan(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LerpOn(this float t, float from, float to) => Mathf.Lerp(from, to, t);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LerpAngleOn(this float t, float from, float to) => Mathf.LerpAngle(from, to, t);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sign(this float value) => Mathf.Sign(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approximately(this float value, float other) => Mathf.Approximately(value, other);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approximately(this float value, float other, float error)
        {
            return Mathf.Abs(value - other) < Mathf.Abs(error);
        }
        
        /// <summary>
        /// 将 x 的值从 [a, b] 区间映射到 [c, d] 区间
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Map(this float x, float a, float b, float c, float d)
        {
            // 避免除零错误
            if (Mathf.Approximately(a, b)) return c;
            return c + (d - c) * (x - a) / (b - a);
        }
        
    }
}