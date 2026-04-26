using System.Collections.Generic;

namespace Zero53.Utils
{
    public static class ArrayUtils
    {
        /// <summary>
        /// 获取二维数组下标迭代器
        /// </summary>
        public static IEnumerable<(int, int)> Indexes<T>(this T[,] array2d)
        {
            for (var i = 0; i < array2d.GetLength(0); i++)
            {
                for (var j = 0; j < array2d.GetLength(1); j++)
                {
                    yield return (i, j);
                }
            }
        }

        /// <summary>
        /// 获取三维数组下标迭代器
        /// </summary>
        public static IEnumerable<(int, int, int)> Indexes<T>(this T[,,] array3d)
        {
            for (var i = 0; i < array3d.GetLength(0); i++)
            {
                for (var j = 0; j < array3d.GetLength(1); j++)
                {
                    for (var k = 0; k < array3d.GetLength(2); k++)
                    {
                        yield return (i, j, k);
                    }
                }
            }
        }

        /// <summary>
        /// 洗牌算法
        /// </summary>
        public static void Shuffle<T>(this T[] array)
        {
            var n = array.Length;
            while (n > 1)
            {
                n--;
                var k = RandomUtils.rng.Next(n + 1);
                (array[k], array[n]) = (array[n], array[k]);
            }
        }
    }
}