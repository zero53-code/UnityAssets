namespace Zero53.BehaviorTree
{
    /// <summary>
    /// 行为树节点接口
    /// </summary>
    public interface INode
    {
        int priority { get; }

        /// <summary>
        /// 过程
        /// </summary>
        /// <returns>状态</returns>
        NodeStatus Process();

        /// <summary>
        /// 递归重置节点和所有子节点
        /// </summary>
        void Reset();
    }
}