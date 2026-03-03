namespace Zero53.BehaviorTree
{
    /// <summary>
    /// 只有一个子节点的行为树节点
    /// </summary>
    public interface ISingleChildNode : INode
    {
        INode child { get; set; }
    }
}