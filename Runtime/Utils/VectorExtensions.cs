using System.Runtime.CompilerServices;
using UnityEngine;

namespace Zero53.Utils
{
    public static class VectorExtensions
    {
        #region Vector3 Extension Mehods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this Vector3 vector)
        {
            return Mathf.Abs(vector.x) < Mathf.Epsilon 
                   && Mathf.Abs(vector.y) < Mathf.Epsilon 
                   && Mathf.Abs(vector.z) < Mathf.Epsilon;
        }

        /// <summary>
        /// 从 from 到 to 的方向向量
        /// </summary>
        /// <returns>to - from</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 To(this Vector3 from, Vector3 to)
        {
            return to - from;
        }

        /// <summary>
        /// 以 up 向量为轴, 判断 forward 向量在 target 左边还是右边
        /// </summary>
        /// <param name="up">基准轴</param>
        /// <param name="forward">操作向量</param>
        /// <param name="target">目标向量</param>
        /// <returns>
        /// 负数表示 forward 在 target 的左边, 正数表示 forward 在 target 的右边.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetSide(this Vector3 up, Vector3 forward, Vector3 target)
        {
            return Vector3.Dot(Vector3.Cross(up, target), forward);
        }

        /// <summary>
        /// 将 from 向量绕着 axis 轴旋转 angle 角度
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Rotate(this Vector3 from, Vector3 axis, float angle)
        {
            return Quaternion.AngleAxis(angle, axis) * from;
        }
        
        #endregion

        #region Vector2 Extension Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this Vector2 vector)
        {
            return Mathf.Abs(vector.x) < Mathf.Epsilon 
                   && Mathf.Abs(vector.y) < Mathf.Epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Rotate(this Vector2 vector, float angle)
        {
            var radian = angle * Mathf.Deg2Rad;
            var cos = Mathf.Cos(radian);
            var sin = Mathf.Sin(radian);
            
            // 使用2D旋转矩阵进行旋转
            var x = cos * vector.x - sin * vector.y;
            var y = sin * vector.x + cos * vector.y;
            
            return new Vector2(x, y);
            // var vec = Quaternion.AngleAxis(angle, Vector3.up) * new Vector3(vector.x, 0, vector.y);
            // return new Vector2(vec.x, vec.z);
        }

        /// <summary>
        /// 计算两个 2D 向量代表的旋转角度叠加后的方向向量
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 AddVectorRotations(this Vector2 a, Vector2 b)
        {
            // 各自的弧度
            var radA = Mathf.Atan2(a.y, a.x);
            var radB = Mathf.Atan2(b.y, b.x);

            // 角度相加
            var totalRad = radA + radB;

            // 生成新方向向量（单位向量）
            return new Vector2(Mathf.Cos(totalRad), Mathf.Sin(totalRad)) * b.normalized;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 To(this Vector2 from, Vector2 to)
        {
            return to - from;
        }
        
        #endregion
    }
}