using UnityEngine;

namespace Zero53.Utils
{
    public static class FloatUtils
    {
        public static float Max(this float a, float b) => Mathf.Max(a, b);
        public static float Min(this float a, float b) => Mathf.Min(a, b);
        public static float Clamp(this float value, float min, float max) => Mathf.Clamp(value, min, max);
        public static float Clamp01(this float value) => Mathf.Clamp01(value);
        public static float Sin(this float value) => Mathf.Sin(value);
        public static float Cos(this float value) => Mathf.Cos(value);
        public static float Tan(this float value) => Mathf.Tan(value);
        public static float Asin(this float value) => Mathf.Asin(value);
        public static float Acos(this float value) => Mathf.Acos(value);
        public static float Atan(this float value) => Mathf.Atan(value);
        public static float LerpOn(this float t, float from, float to) => Mathf.Lerp(from, to, t);
        public static float LerpAngleOn(this float t, float from, float to) => Mathf.LerpAngle(from, to, t);
        
    }
}