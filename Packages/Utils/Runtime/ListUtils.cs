using System;
using System.Collections.Generic;

namespace Zero53.Utils
{
    public static class ListUtils
    {
        private static readonly Random _rng = new();
        
        /// <summary>
        /// 洗牌算法
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = _rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}