
using UnityEngine;

namespace Zero53.Utils
{
    public static class PhysicsUtils
    {
        private static readonly Collider[] _colliderBuffer = new Collider[1];
        
        /// <summary>
        /// 基于 OverlapSphereNonAlloc 的地面检测
        /// </summary>
        /// <param name="position">检测位置</param>
        /// <param name="radius">球体半径</param>
        /// <param name="groundLayerMask">地面的 LayerMask</param>
        /// <returns>是否接触地面</returns>
        public static bool OverlapSphereCheckGround(Vector3 position, float radius, int groundLayerMask)
        {
            var count = Physics.OverlapSphereNonAlloc(
                position,
                radius,
                _colliderBuffer,
                groundLayerMask
            );
            return count > 0;
        }

        /// <summary>
        /// 获取地面的法线
        /// </summary>
        /// <param name="position">检测位置</param>
        /// <param name="checkDistance">检测距离</param>
        /// <param name="groundLayerMask">地面的 LayerMask</param>
        /// <param name="groundNormal">输出地面法线</param>
        /// <returns>
        /// - null: 未检测到地面返回
        /// - not null: 地面法线
        /// </returns>
        public static bool GetGroundNormal(Vector3 position, float checkDistance, int groundLayerMask, out Vector3 groundNormal)
        {
            if (Physics.Raycast(position, Vector3.down, out var hit, checkDistance, groundLayerMask))
            {
                groundNormal = hit.normal;
                return true;
            }

            groundNormal = Vector3.zero;
            return false;
        }
    }
}