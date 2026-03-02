namespace Zero53.BehaviorTree.DecoratorNodes
{
    /// <summary>
    /// 运行直到失败
    /// </summary>
    public class UntilFailNode : DecoratorNode
    {
        public UntilFailNode(string name = "UntilFail", int priority = 0, Node child = null) : base(name, priority, child)
        {
        }

        protected override Status Process()
        {
            if (Children[0].ExecuteProcess() != Status.Failure) return Status.Running;
            
            Reset();
            return Status.Failure;

        }
    }
}