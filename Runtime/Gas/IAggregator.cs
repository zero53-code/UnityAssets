using System.Collections.Generic;

namespace Zero53.Gas
{
    /// <summary>
    /// 聚合器<para/>
    /// </summary>
    public interface IAggregator
    {
        /// <summary>
        /// 对 Modifier 进行聚合计算
        /// </summary>
        /// <param name="baseValue">属性的 baseValue</param>
        /// <param name="modifiers">Modifier 列表</param>
        /// <returns>聚合计算结果</returns>
        float Aggregate(float baseValue, IList<Modifier> modifiers);
    }
}