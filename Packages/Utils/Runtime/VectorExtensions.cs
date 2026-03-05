using UnityEngine;

namespace Zero53.Utils
{
    public static class VectorExtensions
    {
        /// <summary>
        /// 计算由法向量定义的平面上两个向量之间的带符号角
        /// </summary>
        /// <param name="vector1">第一个向量</param>
        /// <param name="vector2">第二个向量</param>
        /// <param name="planeNormal">平面法向量</param>
        /// <returns>向量之间的带符号夹角，单位是度</returns>
        public static float GetAngle(this Vector3 vector1, Vector3 vector2, Vector3 planeNormal)
        {
            var angle = Vector3.Angle(vector1, vector2);
            var sign = Mathf.Sign(Vector3.Dot(planeNormal, Vector3.Cross(vector1, vector2)));
            return angle * sign;
        }
    }
}