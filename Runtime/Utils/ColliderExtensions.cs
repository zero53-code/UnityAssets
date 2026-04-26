using UnityEngine;

namespace Zero53.Utils
{
    public static class ColliderExtensions
    {
        public static void GetWorldCapsuleSize(this CapsuleCollider capsule, out float worldRadius, out float worldHeight)
        {
            var scale = capsule.transform.lossyScale;
            var localRadius = capsule.radius;
            var localHeight = capsule.height;
            var dir = capsule.direction;

            // 计算世界半径（取垂直方向的最大缩放）
            var absX = Mathf.Abs(scale.x);
            var absY = Mathf.Abs(scale.y);
            var absZ = Mathf.Abs(scale.z);

            if (dir == 0) // X轴方向
            {
                worldRadius = localRadius * Mathf.Max(absY, absZ);
                worldHeight = localHeight * absX;
            }
            else if (dir == 1) // Y轴方向
            {
                worldRadius = localRadius * Mathf.Max(absX, absZ);
                worldHeight = localHeight * absY;
            }
            else // Z轴方向
            {
                worldRadius = localRadius * Mathf.Max(absX, absY);
                worldHeight = localHeight * absZ;
            }
        }
    }
}