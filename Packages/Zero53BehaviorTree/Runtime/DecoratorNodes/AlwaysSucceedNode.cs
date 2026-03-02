namespace Zero53.BehaviorTree.DecoratorNodes
{
    /// <summary>
    /// 永远返回 Success
    /// </summary>
    public class AlwaysSucceedNode : DecoratorNode
    {
        public AlwaysSucceedNode(string name = "AlwaysSucceed", int priority = 0, Node child = null) : base(name, priority, child)
        {
        }

        protected override Status Process()
        {
            return Status.Success;
        }
    }
}