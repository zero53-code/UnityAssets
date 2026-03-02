namespace Zero53.BehaviorTree
{
    public interface IAddChildNode
    {
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="child"></param>
        void AddChild(Node child);
    }
}