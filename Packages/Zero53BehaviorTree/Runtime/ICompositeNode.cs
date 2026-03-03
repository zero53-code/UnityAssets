using System.Collections.Generic;

namespace Zero53.BehaviorTree
{
    /// <summary>
    /// 复合节点
    /// 有多个子节点
    /// </summary>
    public interface ICompositeNode : INode
    {
        IList<INode> children { get; }
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="child"></param>
        void AddChild(INode child);
    }
}