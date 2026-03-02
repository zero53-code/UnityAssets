namespace Zero53.BehaviorTree.DecoratorNodes
{
    /// <summary>
    /// 永远返回 Failure
    /// </summary>
    public class AlwaysFailNode : DecoratorNode
    {
        public AlwaysFailNode(string name = "AlwaysFail", int priority = 0, Node child = null) : base(name, priority, child)
        {
        }

        protected override Status Process()
        {
            return Status.Failure;
        }
    }
}